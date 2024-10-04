using BallBlast.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallBlast
{
    public class PowerUpBarrel : PowerUp
    {
        protected override void PickUp()
        {
            HeroManager.Instance.Hero.AddBarrel();
        }
    }
}