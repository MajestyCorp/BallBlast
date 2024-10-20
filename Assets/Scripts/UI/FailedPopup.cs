using BallBlast.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BallBlast.UI
{
    public class FailedPopup : Popup
    {
        [SerializeField]
        private TextMeshProUGUI textScore;

        private void OnEnable()
        {
            var time = ScoreManager.Instance.PassedTime;
            textScore.text = time.ToString("0.00");
        }
    }
}