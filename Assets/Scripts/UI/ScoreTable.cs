using BallBlast.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Leaderboards.Models;
using UnityEngine;

namespace BallBlast.UI
{
    public class ScoreTable : MonoBehaviour
    {
        [SerializeField]
        private PlayerRow rowPrefab;
        [SerializeField]
        private Transform content;
        [SerializeField]
        private GameObject loading;
        [SerializeField]
        private GameObject noRows;
        [SerializeField, Header("UI")]
        private TextMeshProUGUI textLeague;
        [SerializeField, TextArea(1,10)]
        private string nicknames;

        private List<PlayerRow> _items = new();
        private int _itemIndex = 0;
        private string[] _shortNicknames;

        private void Awake()
        {
            _shortNicknames = nicknames.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            Leaderboards.Instance.OnTableUpdated += OnTableUpdated;
            textLeague.gameObject.SetActive(false);
            loading.SetActive(true);
            noRows.SetActive(false);
        }

        private void OnTableUpdated(LeaderboardScores scores)
        {
            ClearItems();
            var tier = InvalidateLeague();
            Build(scores, tier);
        }

        internal string GetShortNickname(string playerName)
        {
            var hash = playerName.GetHashCode();
            return _shortNicknames[Math.Abs(hash) % _shortNicknames.Length];
        }

        private string InvalidateLeague()
        {
            var player = Leaderboards.Instance.PlayerEntry;
            var tier = player != null ? player.Tier : "tier0";

            textLeague.text = GetLeagueRank(tier);
            textLeague.gameObject.SetActive(true);

            return tier;
        }

        private string GetLeagueRank(string tier)
        {
            switch(tier)
            {
                case "tier0":
                    return "Lego Lovers League X";
                case "tier1":
                    return "Block Busters IX";
                case "tier2":
                    return "Plinth Pilots VIII";
                case "tier3":
                    return "Pebble Samurai VII";
                case "tier4":
                    return "Hammer Masters VI";
                case "tier5":
                    return "Reality Wreckers V";
                case "tier6":
                    return "Architect Anarchists IV";
                case "tier7":
                    return "Catapult Crushers III";
                case "tier8":
                    return "Cubic Enforcers II";
                case "tier9":
                    return "Demolition Legends I";
                default:
                    return "";

            }
        }

        private void Build(LeaderboardScores scores, string tier)
        {
            var count = 0;

            if (scores != null)
            {
                var list = scores.Results;

                for (var i = 0; i < list.Count; i++)
                {
                    var entry = list[i];

                    if (entry.Tier.CompareTo(tier) != 0)
                        continue;

                    var item = GetItem();
                    item.Invalidate(this, entry);
                    count++;
                }
            }

            loading.SetActive(false);
            noRows.SetActive(scores == null || count == 0);
        }

        private void ClearItems()
        {
            _itemIndex = 0;
            for (var i = 0; i < _items.Count; i++)
                _items[i].gameObject.SetActive(false);
        }

        private PlayerRow GetItem()
        {
            if (_itemIndex < _items.Count)
            {
                var item = _items[_itemIndex];
                _itemIndex++;
                return item;
            }
            else
            {
                var item = Instantiate(rowPrefab, content);
                item.gameObject.SetActive(false);
                _items.Add(item);
                _itemIndex = _items.Count;
                return item;
            }
        }
    }
}