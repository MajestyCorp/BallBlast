using BallBlast.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Leaderboards.Models;
using UnityEngine;
using UnityEngine.UI;

namespace BallBlast.UI
{
    public class PlayerRow : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI textNumber;
        [SerializeField]
        private TextMeshProUGUI textPlayer;
        [SerializeField]
        private TextMeshProUGUI textScore;
        [SerializeField, Header("UI")]
        private Sprite blockPlayer;
        [SerializeField]
        private Sprite blockOwner;
        [SerializeField]
        private Image image;

        private ScoreTable _table;

        public void Invalidate(ScoreTable table, LeaderboardEntry entry)
        {
            _table = table;
            var rank = entry.Rank + 1;
            int score = (int)entry.Score;
            var nickname = _table.GetShortNickname(entry.PlayerName);

            UpdateText(rank.ToString(), entry.PlayerId, nickname, score);
        }

        private void UpdateText(string rank, string playerId, string playerName, int score)
        {
            var isCurrentPlayer = playerId.CompareTo(Leaderboards.Instance.PlayerId) == 0;

            image.sprite = null;
            image.sprite = isCurrentPlayer ? blockOwner : blockPlayer;

            textNumber.text = rank + ".";
            textPlayer.text = playerName;

            var fScore = score * 0.01f;
            textScore.text = fScore.ToString("0.00");

            this.name = playerName;
            gameObject.SetActive(true);
        }
    }
}