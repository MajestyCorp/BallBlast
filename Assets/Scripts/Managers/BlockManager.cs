using BallBlast.Cameras;
using BallBlast.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallBlast
{
    public class BlockManager : MonoBehaviour
    {
        [System.Serializable]
        private class BlockPrefab
        {
            public Block Prefab;
            [Range(1, 10)]
            public int spawnChance = 10;
        }

        [SerializeField]
        private int liveBlocks = 0;

        [SerializeField, Header("Column Settings")]
        private int columns = 5;
        [SerializeField]
        private float columnWidth = 0.8f;
        [SerializeField]
        private float columnHeight = 0.4f;
        [SerializeField]
        private int rowsOffset = 0;
        [SerializeField, Header("Speed Settings")]
        private float columnSpeed = 1f;
        [SerializeField]
        private float incrementPerSecond = 0.05f;

        [SerializeField, Header("Block Points")]
        private int minPoints = 1;
        [SerializeField]
        private int maxPoints = 2;
        [SerializeField]
        private int incMinPointsEveryRow = 3;
        [SerializeField]
        private int incMaxPointsEveryRow = 2;
        [SerializeField]
        private int pointsMultiplier = 5;

        [SerializeField]
        private Transform blockContent;

        [SerializeField]
        private List<BlockPrefab> availableBlocks;

        private int _rows;
        private int _minPoints;
        private int _maxPoints;
        private float _maxSpawnHeight;
        private float _columnOffsetX;
        private float _lastSpawnHeight = 0;
        private bool _gameStarted = false;
        private float _speed;
        private List<Block> _prefabs = new();
        private Dictionary<Block, Pool> _prefabToPool = new();

        private void Awake()
        {
            EventManager.Instance.OnGameStarted += OnGameStarted;
            EventManager.Instance.OnGameEnded += OnGameEnded;
            Initialize();
        }

        private void Initialize()
        {
            _maxSpawnHeight = ViewPortHandler.Instance.Height * 0.5f + columnHeight - rowsOffset * columnHeight;
            _columnOffsetX = - columns / 2 * columnWidth;

            InitializePrefabs();
        }

        private void InitializePrefabs()
        {
            for(var i=0;i<availableBlocks.Count;i++)
            {
                var data = availableBlocks[i];
                var amount = data.spawnChance;

                for (var j = 0; j < amount; j++)
                    _prefabs.Add(data.Prefab);

                _prefabToPool[data.Prefab] = new Pool(data.Prefab, blockContent);
            }

            _prefabs.Shuffle();
        }

        public void KillBlock()
        {
            liveBlocks--;

            if (_gameStarted && liveBlocks <= 0)
            {
                EventManager.Instance.GameVictory();
                Reset();
            }
        }

        private void Update()
        {
            if (!_gameStarted)
                return;

            if (_lastSpawnHeight + blockContent.position.y < _maxSpawnHeight)
                TryFillRows();

            MoveColumns();
        }

        private void MoveColumns()
        {
            _speed += incrementPerSecond * Time.deltaTime;
            blockContent.localPosition += Vector3.down * _speed * Time.deltaTime;
        }

        private void TryFillRows()
        {
            while(_lastSpawnHeight + blockContent.position.y < _maxSpawnHeight)
            {
                _lastSpawnHeight += columnHeight;
                SpawnRow(_lastSpawnHeight);
            }
        }

        private void SpawnRow(float spawnHeight)
        {
            _rows++;
            InvalidatePoints();

            for(var i=0;i<columns;i++)
            {
                var position = new Vector3(i * columnWidth + _columnOffsetX, spawnHeight, 0);
                var points = Random.Range(_minPoints, _maxPoints + 1) * pointsMultiplier;

                var prefab = _prefabs.Random();
                var pool = _prefabToPool[prefab];
                var block = pool.Take<Block>();
                block.Init(this, position, points);

                liveBlocks++;
            }
        }

        private void InvalidatePoints()
        {
            _minPoints = minPoints + (_rows / incMinPointsEveryRow);
            _maxPoints = maxPoints + (_rows / incMaxPointsEveryRow);
        }

        private void OnGameStarted()
        {
            Reset();
            _gameStarted = true;
        }

        private void Reset()
        {
            _rows = 0;
            _lastSpawnHeight = 0;
            _speed = columnSpeed;
            blockContent.localPosition = Vector3.zero;

            var count = blockContent.childCount;

            for(var i=0;i<count;i++)
            {
                var child = blockContent.GetChild(i);
                if (!child.gameObject.activeSelf)
                    continue;

                child.gameObject.Release();
            }

            liveBlocks = 0;
        }

        private void OnGameEnded(bool victory)
        {
            _gameStarted = false;
            StopAllCoroutines();
        }

        private void OnDestroy()
        {
            EventManager.Instance.OnGameStarted -= OnGameStarted;
            EventManager.Instance.OnGameEnded -= OnGameEnded;
        }
    }
}