using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallBlast
{
    public class PowerUpFirerate : PowerUp
    {
        [SerializeField, Range(1f, 2f)]
        private float firerateMultiplier = 1.1f;

        protected override void PickUp()
        {
            ShooterManager.Instance.ApplyFirerate(firerateMultiplier);
        }
    }
}