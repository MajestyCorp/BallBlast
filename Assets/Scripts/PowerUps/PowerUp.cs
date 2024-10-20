using BallBlast.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallBlast
{
    public abstract class PowerUp : MonoBehaviour
    {
        [SerializeField]
        private AnimationCurve curve;
        [SerializeField]
        private float lifetime = 1;
        [SerializeField]
        private GameObject model;
        
        private bool _grounded;

        protected abstract void PickUp();

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.CompareTag("Player"))
            {
                SoundManager.Instance.PickUp();
                PickUp();
                Destroy(gameObject);
            } else if(!_grounded)
            {
                _grounded = true;
                StartCoroutine(Blinker());
            }
        }

        private IEnumerator Blinker()
        {
            bool value;
            Timer timer = new();
            timer.Activate(lifetime);

            while(!timer.IsFinished)
            {
                yield return null;
                value = curve.Evaluate(timer.Progress) > 0.5f;
                if (model.activeSelf != value)
                    model.SetActive(value);
            }

            Destroy(gameObject);
        }

        private void OnEnable()
        {
            EventManager.Instance.OnGameStarted += OnGameStarted;
        }

        private void OnGameStarted()
        {
            Destroy(gameObject);
        }

        private void OnDisable()
        {
            EventManager.Instance.OnGameStarted -= OnGameStarted;
        }
    }
}