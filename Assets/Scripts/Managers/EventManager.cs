using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallBlast.Managers
{
    public class EventManager : MonoBehaviour, IInitializer
    {
        public static EventManager Instance { get; private set; }

        public delegate void VoidHandler();
        public delegate void PauseHandler(bool paused);
        public delegate void InitHandler(bool firstTime);
        public delegate void VictoryHandler(bool victory);
        public event InitHandler OnGameInitialized;
        public event VoidHandler OnGameStarted;
        public event PauseHandler OnGamePaused;
        public event VictoryHandler OnGameEnded;

        public void InitializeAfter()
        {
        }

        public void InitializeSelf()
        {
            Instance = this;
        }

        private void Start()
        {
            Time.timeScale = 0f;
            OnGameInitialized?.Invoke(true);
        }

        public void GameInitialized()
        {
            Time.timeScale = 0f;
            OnGameInitialized?.Invoke(false);
        }

        public void GameStarted()
        {
            Time.timeScale = 1f;
            OnGameStarted?.Invoke();
        }

        public void GamePaused(bool paused)
        {
            Time.timeScale = paused ? 0f : 1f;
            OnGamePaused?.Invoke(paused);
        }

        public void GameVictory()
        {
            Time.timeScale = 0f;
            OnGameEnded?.Invoke(true);
        }

        public void GameFailed()
        {
            Time.timeScale = 0f;
            OnGameEnded?.Invoke(false);
        }
    }
}