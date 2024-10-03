using BallBlast.Cameras;
using BallBlast.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BallBlast
{
    public class HeroController : MonoBehaviour
    {
        [SerializeField]
        private float halfWidth = 1.5f;
        [SerializeField]
        private float steeringSpeed = 1f;

        private Transform _heroTransform;
        private Hero _hero;
        private Vector3 _lastMousePos;

        private void Awake()
        {
            EventManager.Instance.OnGameStarted += OnGameStarted;

            Initialize();
        }

        private void Initialize()
        {
            _hero = HeroManager.Instance.Hero;
            _heroTransform = _hero.transform;

            _heroTransform.localPosition = Vector3.zero;
        }

        private void OnGameStarted()
        {
            _heroTransform.localPosition = Vector3.zero;
        }

        private void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                _lastMousePos = Input.mousePosition;
            }

            if(Input.GetMouseButton(0))
            {
                var delta = Input.mousePosition - _lastMousePos;
                _lastMousePos = Input.mousePosition;

                var newPosX = _heroTransform.localPosition.x + delta.x * Time.deltaTime * steeringSpeed / Screen.width;
                newPosX = Mathf.Clamp(newPosX, -halfWidth + _hero.HalfColliderWidth, halfWidth - _hero.HalfColliderWidth);

                _heroTransform.localPosition = Vector2.right * newPosX;
            }
            /*
            if(Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                var newXPos = _heroTransform.localPosition.x + touch.deltaPosition.x * Time.deltaTime * steeringSpeed / Screen.width;
                newXPos = Mathf.Clamp(newXPos, -halfWidth, halfWidth);
                _heroTransform.localPosition = Vector2.right * newXPos;
            }*/
        }

        private void OnDestroy()
        {
            if(EventManager.Instance != null)
                EventManager.Instance.OnGameStarted -= OnGameStarted;
        }
    }
}