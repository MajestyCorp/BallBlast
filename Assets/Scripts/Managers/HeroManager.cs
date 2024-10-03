using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallBlast.Managers
{
    public class HeroManager : MonoBehaviour, IInitializer
    {
        public static HeroManager Instance { get; private set; }
        public Hero Hero => _hero;

        [SerializeField]
        private Hero heroPrefab;
        [SerializeField]
        private Transform heroContainer;

        private Hero _hero;


        public void InitializeAfter()
        {
            EventManager.Instance.OnGameStarted += OnGameStarted;
            Initialize();
        }

        public void InitializeSelf()
        {
            Instance = this;
        }

        private void Initialize()
        {
            _hero = Instantiate(heroPrefab, heroContainer);
            _hero.gameObject.SetActive(false);
        }

        private void OnGameStarted()
        {
            _hero.gameObject.SetActive(false);
            _hero.Reset();
            _hero.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            if (EventManager.Instance != null)
                EventManager.Instance.OnGameStarted -= OnGameStarted;
        }
    }
}