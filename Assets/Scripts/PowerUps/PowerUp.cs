using BallBlast.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallBlast
{
    public abstract class PowerUp : MonoBehaviour
    {
        protected abstract void PickUp();

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.CompareTag("Player"))
            {
                PickUp();
                Destroy(gameObject);
            }
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