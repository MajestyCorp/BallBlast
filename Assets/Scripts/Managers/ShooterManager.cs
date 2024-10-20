using BallBlast.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace BallBlast
{
    public class ShooterManager : MonoBehaviour, IInitializer
    {
        public static ShooterManager Instance { get; private set; }

        
        [SerializeField, Header("Bullet Settings")]
        private float initialShotDelay = 0.1f;
        [SerializeField]
        private float minShotDelay = 0.005f;

        private float _shotDelay;

        internal void ApplyFirerate(float multiplier)
        {
            _shotDelay = Mathf.Max(minShotDelay, _shotDelay / multiplier);
        }

        private List<Transform> _barrels = new();
        private float _lastShotTime;
        private List<Vector3> _shotPositions = new();

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
            EventManager.Instance.OnGameStarted += OnGameStarted;
        }

        private void OnGameStarted()
        {
            _shotDelay = initialShotDelay;
            _lastShotTime = Time.time;
        }

        public void RegisterBarrel(Transform barrel)
        {
            _barrels.Add(barrel);
        }

        public void DeregisterBarrel(Transform barrel)
        {
            _barrels.Remove(barrel);
        }

        private void Update()
        {
            if (Time.timeScale < 0.5f)
                return;

            while (_lastShotTime + _shotDelay <= Time.time)
            {
                DoShoot(_lastShotTime + _shotDelay);
                _lastShotTime += _shotDelay;
            }
        }

        private void DoShoot(float shootTime)
        {
            var delta = Time.time - shootTime;
            var deltaMove = Vector3.up * delta * BulletManager.Instance.BulletSpeed;

            _shotPositions.Clear();

            for (var i=0;i<_barrels.Count;i++)
            {
                var barrel = _barrels[i];
                _shotPositions.Add(barrel.position + deltaMove);
                
            }

            BulletManager.Instance.CreateBullets(_shotPositions);
            SoundManager.Instance.Shot();
        }

        private void OnDestroy()
        {
            if(EventManager.Instance != null)
                EventManager.Instance.OnGameStarted -= OnGameStarted;
        }
    }
}