using BallBlast.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallBlast.Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private Frame menuUI;
        [SerializeField]
        private Frame victoryUI;
        [SerializeField]
        private Frame failedUI;
        [SerializeField]
        private Frame gameUI;
        [SerializeField, Header("Buttons")]
        private GameObject buttonStart;
        [SerializeField]
        private GameObject buttonResume;
        [SerializeField]
        private GameObject buttonRestart;

        private bool _gameStarted = false;
        private bool _gameEnded = false;

        private void Awake()
        {
            EventManager.Instance.OnGameInitialized += OnGameInitialized;
            EventManager.Instance.OnGameStarted += OnGameStarted;
            EventManager.Instance.OnGamePaused += OnGamePaused;
            EventManager.Instance.OnGameEnded += OnGameEnded;
        }

        private void OnGameInitialized(bool firstTime)
        {
            _gameStarted = false;
            _gameEnded = false;
            ShowMenu(menuUI);
        }

        private void OnGameStarted()
        {
            _gameStarted = true;
            _gameEnded = false;
            ShowMenu(gameUI);
        }

        private void OnGamePaused(bool paused)
        {
            ShowMenu(paused ? menuUI : gameUI);
        }

        private void OnGameEnded(bool victory)
        {
            _gameEnded = true;
            ShowMenu(victory ? victoryUI : failedUI);
        }

        private void ShowMenu(Frame menu)
        {
            gameUI.Toggle(menu == gameUI);
            menuUI.Toggle(menu == menuUI);
            victoryUI.Toggle(menu == victoryUI);
            failedUI.Toggle(menu == failedUI);

            if(menuUI.IsVisible)
            {
                buttonResume.SetActive(_gameStarted && !_gameEnded);
                buttonRestart.SetActive(_gameStarted || _gameEnded);
                buttonStart.SetActive(!_gameStarted && !_gameEnded);
            }
        }

        public void ButtonStart()
        {
            SoundManager.Instance.ButtonClick();
            EventManager.Instance.GameStarted();
        }

        public void ButtonResume()
        {
            SoundManager.Instance.ButtonClick();
            EventManager.Instance.GamePaused(false);
        }

        public void ButtonRestart()
        {
            SoundManager.Instance.ButtonClick();
            EventManager.Instance.GameStarted();
        }

        public void ButtonQuit()
        {
            SoundManager.Instance.ButtonClick();
            Application.Quit();
        }

        public void ButtonPause()
        {
            SoundManager.Instance.ButtonClick();
            EventManager.Instance.GamePaused(true);
        }

        public void ButtonGoToMenu()
        {
            SoundManager.Instance.ButtonClick();
            EventManager.Instance.GameInitialized();
        }

        private void OnDestroy()
        {
            if (EventManager.Instance != null)
            {
                EventManager.Instance.OnGameInitialized -= OnGameInitialized;
                EventManager.Instance.OnGameStarted -= OnGameStarted;
                EventManager.Instance.OnGamePaused -= OnGamePaused;
                EventManager.Instance.OnGameEnded -= OnGameEnded;
            }
        }
    }
}