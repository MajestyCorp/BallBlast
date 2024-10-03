using BallBlast.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace BallBlast
{
    public class ShooterManager : MonoBehaviour, IInitializer
    {
        public static ShooterManager Instance { get; private set; }

        
        [SerializeField, Header("Bullet Settings")]
        private float shotDelay = 0.1f;

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
            while (_lastShotTime + shotDelay <= Time.time)
            {
                DoShoot(_lastShotTime + shotDelay);
                _lastShotTime += shotDelay;
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