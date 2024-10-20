using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityEngine;

namespace BallBlast.Managers
{
    public class Leaderboards : MonoBehaviour, IInitializer
    {

        public delegate void EntryHandler(LeaderboardEntry entry);
        public delegate void PageHandler(LeaderboardScoresPage page);
        public delegate void ScoresHandler(LeaderboardScores scores);

        public event EntryHandler OnInitialize;
        public event EntryHandler OnScoreUpdated;
        public event ScoresHandler OnTableUpdated;

        public static Leaderboards Instance { get; private set; }

        public const string ID_TIME = "time";

        public string Nickname { get; private set; }
        public string PlayerId { get; private set; }
        public LeaderboardEntry PlayerEntry { get; private set; }
        //public LeaderboardScoresPage TableScores { get; private set; }
        public LeaderboardScores TableScores { get; private set; }

        [SerializeField]
        private List<Sprite> monsters;

        private const string _leaderboardId = "yearly";
        private const string _nicknameId = "Nickname";
        private bool _initialized = false;

        public void InitializeAfter()
        {
            EventManager.Instance.OnGameEnded += OnGameEnded;
        }

        public void InitializeSelf()
        {
            Instance = this;
        }

        public Sprite GetPlayerIcon(string playerId)
        {
            return monsters[Mathf.Abs(playerId.GetHashCode()) % monsters.Count];
        }

        public Color GetPlayerColor(string playerId)
        {
            var hash = Mathf.Abs(playerId.GetHashCode());
            var hue = (hash % 256) / 256f;

            return Color.HSVToRGB(hue, 1f, 1f);
        }

        private async void Awake()
        {
            await UnityServices.InitializeAsync();

            await SignInAnonymously();
        }

        private async Task SignInAnonymously()
        {
            AuthenticationService.Instance.SignedIn += async () =>
            {
                _initialized = true;
                PlayerId = AuthenticationService.Instance.PlayerId;
                await InitializeName();
                await InitPlayerAsync();
                GetPageScores();

#if UNITY_EDITOR
                Debug.Log("Signed In as " + PlayerId);
#endif
            };
            AuthenticationService.Instance.SignInFailed += s =>
            {
                // Take some action here...
                Debug.Log(s);
            };

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        private async Task InitializeName()
        {
            if (CustomPlayerPrefs.HasKey(_nicknameId))
            {
                Nickname = CustomPlayerPrefs.GetString(_nicknameId);
            }
            else
            {
                try
                {
                    Nickname = await AuthenticationService.Instance.GetPlayerNameAsync();
                    CustomPlayerPrefs.SetString(_nicknameId, Nickname);
                    CustomPlayerPrefs.Save();
                }
                catch (Exception ex)
                {
                    _initialized = false;
                    Debug.LogException(ex, this);
                }
            }
        }

        private async Task InitPlayerAsync()
        {
            try
            {
                var scoreResponse = await LeaderboardsService.Instance.GetPlayerScoreAsync(
                    _leaderboardId
                    //,                    new GetPlayerScoreOptions { IncludeMetadata = true }
                    );

                PlayerEntry = scoreResponse;
                OnInitialize?.Invoke(scoreResponse);
            }
            catch (Exception ex)
            {
                //no player data
                PlayerEntry = null;
                OnInitialize?.Invoke(null);
            }
        }

        private async void GetPageScores()
        {
            try
            {
                TableScores = await LeaderboardsService.Instance.GetPlayerRangeAsync(_leaderboardId, new GetPlayerRangeOptions { RangeLimit = 20 });
                //TableScores = await LeaderboardsService.Instance.GetScoresAsync(_leaderboardId, new GetScoresOptions { Offset = 0, Limit = 100 });
                OnTableUpdated?.Invoke(TableScores);
            }
            catch (Exception ex)
            {
                //no player data
                TableScores = null;
                OnTableUpdated?.Invoke(null);
            }
        }

        private async void OnGameEnded(bool victory)
        {
            if (!_initialized || !victory)
                return;

            var score = ScoreManager.Instance.Scores;

            if (score > 0)
            {
                var metadata = new Dictionary<string, string>();
                var value = score / 100f;

                metadata[ID_TIME] = value.ToString("0.00");

                var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(_leaderboardId, score);

                //var scoreResponse = await LeaderboardsService.Instance.GetPlayerScoreAsync(_leaderboardId);

                PlayerEntry = scoreResponse;
                OnScoreUpdated?.Invoke(scoreResponse);

                GetPageScores();
            }
        }

        private void OnDestroy()
        {
            if (EventManager.Instance != null)
                EventManager.Instance.OnGameEnded -= OnGameEnded;
        }
    }
}