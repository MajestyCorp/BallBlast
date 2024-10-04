using BallBlast.Cameras;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallBlast.Managers
{
    public class BulletManager : MonoBehaviour, IInitializer
    {
        public static BulletManager Instance { get; private set; }
        private const int MaxBulletsInPack = 1023;
        public float BulletSpeed { get; private set; }

        [SerializeField]
        private Sprite bulletSprite;
        [SerializeField]
        private Material bulletMaterial;
        [SerializeField]
        private float bulletSpeed = 1f;
        [SerializeField]
        private LayerMask hitLayer;

        private Mesh _bulletMesh;
        private List<List<BulletData>> _bulletPacks = new();
        private List<Matrix4x4> _matrices = new();
        private float _maxScreenHeight;


        private struct BulletData
        {
            public Vector3 Position { get; }
            public float SpawnTime { get; }
            public bool Exploded { get; }
            public bool Died { get; }

            public BulletData(Vector3 position, float spawnTime)
            {
                Exploded = Died = false;
                Position = position;
                SpawnTime = spawnTime;
            }

            public BulletData(Vector3 position, float spawnTime, bool died) : this(position, spawnTime)
            {
                Died = died;
            }

            public BulletData(Vector3 position, float spawnTime, bool died, bool exploded) : this(position, spawnTime, died)
            {
                Exploded = exploded;
            }

            public BulletData Die()
            {
                return new BulletData(Position, SpawnTime, true);
            }

            public BulletData Explode()
            {
                return new BulletData(Position, SpawnTime, true, true);
            }
        }

        public void InitializeAfter()
        {
        }

        public void InitializeSelf()
        {
            Instance = this;
            Initialize();
        }

        private void Initialize()
        {
            _maxScreenHeight = ViewPortHandler.Instance.Height;
            _bulletMesh = SpriteToMesh(bulletSprite);

            EventManager.Instance.OnGameStarted += OnGameStarted;
        }

        private void Update()
        {
            //if (Time.timeScale < 0.5f)
            //    return;

            for(var i=0;i<_bulletPacks.Count;i++)
            {
                var pack = _bulletPacks[i];
                ProcessPack(pack);
            }
        }

        private void ProcessPack(List<BulletData> pack)
        {
            _matrices.Clear();
            var raycastDistance = Time.deltaTime * BulletSpeed;
            RaycastHit2D rayHit;

            for (var i=0;i<pack.Count;i++)
            {
                var data = pack[i];
                var passedTime = Time.time - data.SpawnTime;
                var dist = passedTime * BulletSpeed;
                var pos = data.Position + Vector3.up * dist;

                if (dist > _maxScreenHeight)
                {
                    pack[i] = data.Die();
                } else
                {
                    rayHit = Physics2D.Raycast(pos, Vector2.up, raycastDistance, hitLayer);
                    if(rayHit.collider != null && rayHit.collider.TryGetComponent(out Block block))
                    {
                        block.DoHit();
                        pack[i] = data.Explode();
                        SoundManager.Instance.Hit();
                    }
                }

                _matrices.Add(Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one));
            }

            Graphics.DrawMeshInstanced(_bulletMesh, 0, bulletMaterial, _matrices);

            for (var i = pack.Count - 1; i >= 0; i--)
            {
                var data = pack[i];
                if (data.Died)
                    pack.RemoveAt(i);
            }
        }

        private void OnGameStarted()
        {
            BulletSpeed = bulletSpeed;

            for (var i=0;i<_bulletPacks.Count;i++)
            {
                var pack = _bulletPacks[i];
                pack.Clear();
            }
        }

        private Mesh SpriteToMesh(Sprite sprite)
        {
            return new Mesh
            {
                vertices = Array.ConvertAll(sprite.vertices, i => (Vector3)i),
                uv = sprite.uv,
                triangles = Array.ConvertAll(sprite.triangles, i => (int)i)
            };
        }

        public void CreateBullets(List<Vector3> positions)
        {
            for(var i=0;i<_bulletPacks.Count;i++)
            {
                var pack = _bulletPacks[i];

                if(pack.Count + positions.Count < MaxBulletsInPack)
                {
                    CreateBulletsInPack(pack, positions);
                    return;
                }
            }

            var newPack = new List<BulletData>();
            CreateBulletsInPack(newPack, positions);
            _bulletPacks.Add(newPack);
        }

        private void CreateBulletsInPack(List<BulletData> pack, List<Vector3> positions)
        {
            for(var i=0;i<positions.Count;i++)
            {
                var data = new BulletData(positions[i], Time.time);
                pack.Add(data);
            }
        }

        private void OnDestroy()
        {
            if (EventManager.Instance != null)
                EventManager.Instance.OnGameStarted -= OnGameStarted;
        }
    }
}