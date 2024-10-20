using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BallBlast.Managers
{
    public class ScoreManager : MonoBehaviour, IInitializer
    {
        public static ScoreManager Instance { get; private set; }
        public int Scores => (int)((Time.time - _startTime) * 100);
        public float PassedTime => (Time.time - _startTime);

        [SerializeField]
        private string formatText = "{0} s";
        [SerializeField]
        private TextMeshProUGUI textScore;

        private float _startTime;
        private float _delta;

        public void InitializeSelf()
        {
            Instance = this;
        }

        public void InitializeAfter()
        {
            EventManager.Instance.OnGameStarted += OnGameStarted;
        }

        private void OnGameStarted()
        {
            _startTime = Time.time;
        }

        private void Update()
        {
            _delta = Time.time - _startTime;
            textScore.text = string.Format(formatText, _delta.ToString("0.00"));
        }

        private void OnDestroy()
        {
            if (EventManager.Instance != null)
            {
                EventManager.Instance.OnGameStarted -= OnGameStarted;
            }
        }
    }
}