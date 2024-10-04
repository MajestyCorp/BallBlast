using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallBlast
{
    public class Hero : MonoBehaviour
    {
        public float HalfColliderWidth { get; private set; }

        [SerializeField, Min(1)]
        private int initialBarrels = 1;
        [SerializeField]
        private float width = 0.22f;
        [SerializeField]
        private float colliderXPadding;
        [SerializeField]
        private Transform planeLeft;
        [SerializeField]
        private Transform planeMiddle;
        [SerializeField]
        private Transform planeRight;
        [SerializeField]
        private BoxCollider2D boxCollider;

        private List<Transform> _middles = new();
        private int _barrels = 1;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _middles.Add(planeMiddle);
        }

        private void OnEnable()
        {
            Reset();
        }

        public void Reset()
        {
            _barrels = initialBarrels;
            InvalidateBarrels();
        }

        public void AddBarrel()
        {
            _barrels++;
            InvalidateBarrels();
        }

        private void InvalidateBarrels()
        {
            SyncAmount();
            SyncPositions();
        }

        private void SyncAmount()
        {
            while(_middles.Count > _barrels)
            {
                var barrel = _middles[^1];
                Destroy(barrel.gameObject);
                _middles.RemoveAt(_middles.Count-1);
            }

            if(_middles.Count < _barrels)
            {
                var addAmount = _barrels - _middles.Count;
                for(var i=0;i<addAmount;i++)
                {
                    var barrel = Instantiate(planeMiddle, planeMiddle.parent);
                    _middles.Add(barrel);
                }
            }
        }

        private void SyncPositions()
        {
            var offset = _middles.Count % 2 == 0 ? Vector2.right * width / 2 : Vector2.zero;
            var half = _middles.Count / 2;

            for(var i=0;i<_middles.Count;i++)
            {
                var barrel = _middles[i];
                barrel.localPosition = Vector2.right * (-half + i) * width + offset;
            }

            planeLeft.localPosition = Vector2.right * (-half - 1) * width + offset;
            planeRight.localPosition = Vector2.right * (-half + _middles.Count) * width + offset;

            var boxSize = boxCollider.size;
            boxSize.x = (_middles.Count + 2) * width - colliderXPadding;
            boxCollider.size = boxSize;

            HalfColliderWidth = boxSize.x / 2;
        }
    }
}