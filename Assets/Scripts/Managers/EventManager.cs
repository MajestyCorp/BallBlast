using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallBlast.Managers
{
    public class EventManager : MonoBehaviour, IInitializer
    {
        public static EventManager Instance { get; private set; }

        public delegate void VoidHandler();
        public event VoidHandler OnGameStarted;
        public event VoidHandler OnGameFailed;

        public void InitializeAfter()
        {
        }

        public void InitializeSelf()
        {
            Instance = this;
        }

        public void GameStarted()
        {
            OnGameStarted?.Invoke();
        }

        public void GameFailed()
        {
            OnGameFailed?.Invoke();
        }
    }
}