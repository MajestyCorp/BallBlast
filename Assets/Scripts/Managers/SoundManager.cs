using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallBlast.Managers
{
    public class SoundManager : MonoBehaviour, IInitializer
    {
        public static SoundManager Instance { get; private set; }

        [SerializeField]
        private AudioSource audioSource;

        [SerializeField, Header("Clips")]
        private AudioClip buttonClick;
        [SerializeField]
        private AudioClip shot;
        [SerializeField]
        private AudioClip hit;
        [SerializeField]
        private float minClipDelay = 0.1f;

        private bool _playShot;
        private bool _playHit;
        private float _shotTime;
        private float _hitTime;

        public void InitializeAfter()
        {
        }

        public void InitializeSelf()
        {
            Instance = this;
        }

        public void ButtonClick()
        {
            audioSource.PlayOneShot(buttonClick);
        }

        public void Shot()
        {
            _playShot = true;
        }

        public void Hit()
        {
            _playHit = true;
        }

        private void Update()
        {
            TryPlayClip(ref _playShot, ref _shotTime, shot);
            TryPlayClip(ref _playHit, ref _hitTime, hit);
        }

        private void TryPlayClip(ref bool needPlay, ref float time, AudioClip clip)
        {
            if (!needPlay || time > Time.time)
                return;

            audioSource.PlayOneShot(clip);
            needPlay = false;
            time = Time.time + minClipDelay;
        }
    }
}