using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallBlast
{
    public class Barrel : MonoBehaviour
    {
        private void OnEnable()
        {
            ShooterManager.Instance.RegisterBarrel(transform);
        }

        private void OnDisable()
        {
            ShooterManager.Instance.DeregisterBarrel(transform);
        }
    }
}