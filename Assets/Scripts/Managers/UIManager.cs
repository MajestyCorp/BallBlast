using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallBlast.Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject menuUI;
        [SerializeField]
        private GameObject gameUI;
        [SerializeField, Header("Buttons")]
        private GameObject buttonStart;
        [SerializeField]
        private GameObject buttonResume;
        [SerializeField]
        private GameObject buttonRestart;

        private bool _gameStarted = false;
        private bool _gameFailed = false;

        private void Awake()
        {
            EventManager.Instance.OnGameFailed += OnGameFailed;

            ShowMenu(true);
        }

        private void OnGameFailed()
        {
            _gameFailed = true;
            ShowMenu(true);
        }

        private void ShowMenu(bool value)
        {
            Time.timeScale = value ? 0f : 1f;
            menuUI.SetActive(value);
            gameUI.SetActive(!value);

            if(value)
            {
                buttonStart.SetActive(!_gameStarted);
                buttonResume.SetActive(_gameStarted && !_gameFailed);
                buttonRestart.SetActive(_gameStarted);
            }
        }

        public void ButtonStart()
        {
            SoundManager.Instance.ButtonClick();
            _gameStarted = true;
            _gameFailed = false;
            ShowMenu(false);
            EventManager.Instance.GameStarted();
        }

        public void ButtonResume()
        {
            SoundManager.Instance.ButtonClick();
            ShowMenu(false);
        }

        public void ButtonRestart()
        {
            SoundManager.Instance.ButtonClick();

            _gameFailed = false;
            ShowMenu(false);
            EventManager.Instance.GameStarted();
        }

        public void ButtonQuit()
        {
            SoundManager.Instance.ButtonClick();
            Application.Quit();
        }

        public void ButtonMenu()
        {
            SoundManager.Instance.ButtonClick();
            ShowMenu(true);
        }

        private void OnDestroy()
        {
            if (EventManager.Instance != null)
                EventManager.Instance.OnGameFailed -= OnGameFailed;
        }
    }
}