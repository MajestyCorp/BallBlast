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
        private const float KillAnimationTime = 0.2f;

        [SerializeField]
        private TextMeshPro text;
        [SerializeField]
        private PowerUp powerUpPrefab;

        private bool _killed;
        private int _points;
        private BlockManager _manager;

        public void Init(BlockManager manager, Vector3 position, int points)
        {
            _manager = manager; 
            transform.localScale = Vector3.one;
            _points = points;
            _killed = false;
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
            _manager.KillBlock();

            if (powerUpPrefab != null)
                SpawnPowerUp();

            transform.DOScale(Vector3.zero, KillAnimationTime)
                .SetEase(Ease.InOutSine)
                .OnComplete(() => OnDieComplete())
                .SetTarget(transform);
        }

        private void OnDieComplete()
        {
            gameObject.Release();
        }

        private void OnDisable()
        {
            if (_points <= 0)
                transform.DOKill();
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