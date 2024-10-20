using BallBlast.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BallBlast.UI
{
    public class VictoryPopup : Popup
    {
        [SerializeField]
        private TextMeshProUGUI textLeague;
        [SerializeField]
        private TextMeshProUGUI textTip;
        [SerializeField]
        private TextMeshProUGUI textScore;
        [SerializeField, Header("Stars")]
        private GameObject star2;
        [SerializeField]
        private GameObject star3;

        private void OnEnable()
        {
            var time = ScoreManager.Instance.PassedTime;
            textScore.text = time.ToString("0.00");

            textLeague.text = GetLeagueText(time, out var tip);
            textTip.text = tip;

            InvalidateStars(time);
        }

        private void InvalidateStars(float time)
        {
            star2.SetActive(time >= 30f);
            star3.SetActive(time >= 45f);
        }

        private string GetLeagueText(float time, out string tip)
        {
            if (time >= 180f)
            {
                tip = "Legends aren't built—they’re shattered!";
                return "Demolition Legends I";
            }
            if (time >= 150f)
            {
                tip = "We keep the blocks in line... and then smash 'em.";
                return "Cubic Enforcers II";
            }
            if (time >= 120f)
            {
                tip = "Flinging chaos since day one.";
                return "Catapult Crushers III";
            }
            if (time >= 105f)
            {
                tip = "Design? No thanks. We demolish.";
                return "Architect Anarchists IV";
            }
            if (time >= 90f)
            {
                tip = "Destroying worlds, one pixel at a time.";
                return "Reality Wreckers V";
            }
            if (time >= 75f)
            {
                tip = "If all you have is a hammer... everything's a block!";
                return "Hammer Masters VI";
            }
            if (time >= 60f)
            {
                tip = "Tiny stones, big destruction!";
                return "Pebble Samurai VII";
            }
            if (time >= 45f)
            {
                tip = "We aim low, but we hit hard.";
                return "Plinth Pilots VIII";
            }
            if (time >= 30f)
            {
                tip = "No block left unbusted!";
                return "Block Busters IX";
            }
            else
            {
                tip = "Where breaking bricks is still a hobby.";
                return "Lego Lovers League X";
            }
        }
    }
}