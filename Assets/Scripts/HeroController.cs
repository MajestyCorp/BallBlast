using BallBlast.Cameras;
using BallBlast.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace BallBlast
{
    public class HeroController : MonoBehaviour
    {
        [SerializeField]
        private float halfWidth = 2.5f;
        [SerializeField]
        private float lerpSpeed = 30f;

        private Hero _hero;
        private Vector3 _desiredPosition;
        private Transform _playerTransform;
        private bool _handling;

        private void Awake()
        {
            EventManager.Instance.OnGameStarted += OnGameStarted;

            Initialize();
        }

        private void Initialize()
        {
            _hero = HeroManager.Instance.Hero;
            _playerTransform = _hero.transform;
            _desiredPosition = Vector3.zero;
            _handling = false;
        }

        private void OnGameStarted()
        {
            _desiredPosition = Vector3.zero;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                _handling = true;
            }

            if (_handling && Input.GetMouseButton(0))
            {
                InvalidateDesiredPosition(Input.mousePosition);
            }

            if (Input.GetMouseButtonUp(0))
            {
                _handling = false;
            }

            LerpPlayerPosition();
        }

        private void LerpPlayerPosition()
        {
            _playerTransform.localPosition = Vector3.Lerp(_playerTransform.localPosition, _desiredPosition, lerpSpeed * Time.deltaTime);
        }

        private void InvalidateDesiredPosition(Vector3 mousePosition)
        {
            var width = ViewPortHandler.Instance.Width;
            var relativeX = Mathf.Clamp01(mousePosition.x / Screen.width) - 0.5f;
            var offset = _hero.HalfColliderWidth;
            var leftOffset = Mathf.Min(-halfWidth + offset, 0f);
            var rightOffset = Mathf.Max(halfWidth - offset, 0f);
            var gameX = Mathf.Clamp(relativeX * width, leftOffset, rightOffset);

            _desiredPosition = new Vector3(gameX, 0f, 0f);
        }

        private void OnDestroy()
        {
            if(EventManager.Instance != null)
                EventManager.Instance.OnGameStarted -= OnGameStarted;
        }
    }
}