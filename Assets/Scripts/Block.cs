using BallBlast.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

namespace BallBlast
{
    public class Block : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro text;
        [SerializeField]
        private PowerUp powerUpPrefab;

        private int _points;

        public void Init(Vector3 position, int points)
        {
            transform.localScale = Vector3.one;
            _points = points;
            transform.localPosition = position;
            text.gameObject.SetActive(true);
            InvalidateText();

            gameObject.SetActive(true);
        }

        public void DoHit()
        {
            _points--;

            InvalidateText();

            if (_points == 0)
                Die();
        }

        private void Die()
        {
            if (powerUpPrefab != null)
                SpawnPowerUp();

            transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InOutSine).OnComplete(() => OnDieComplete());
        }

        private void OnDieComplete()
        {
            gameObject.Release();
        }

        private void SpawnPowerUp()
        {
            var powerup = Instantiate(powerUpPrefab);
            powerup.transform.position = transform.position;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            EventManager.Instance.GameFailed();
        }

        private void InvalidateText()
        {
            if(_points > 0)
            {
                text.text = _points.ToString();
            } else if(text.gameObject.activeSelf)
            {
                text.gameObject.SetActive(false);
            }
        }
    }
}