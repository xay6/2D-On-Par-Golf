using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using OnPar.RouterHandlers;
using OnPar.Routers;

namespace Unity.Template.Multiplayer.NGO.Runtime
{
    internal class LeaderboardController : Controller<MetagameApplication>
    {
        LeaderboardView View => App.View.Leaderboard;

        void Awake()
        {
            AddListener<EnterLeaderboardEvent>(OnEnterLeaderboard);
            AddListener<ExitLeaderboardEvent>(OnExitLeaderboard);
        }

        void OnDestroy()
        {
            RemoveListeners();
        }

        internal override void RemoveListeners()
        {
            RemoveListener<EnterLeaderboardEvent>(OnEnterLeaderboard);
            RemoveListener<ExitLeaderboardEvent>(OnExitLeaderboard);
        }

        void OnEnterLeaderboard(EnterLeaderboardEvent evt)
        {
            View.SetCurrentUsername(App.CurrentUsername);
            PopulateLeaderboard();
            View.Show();
        }

        void OnExitLeaderboard(ExitLeaderboardEvent evt)
        {
            View.Hide();
        }

        async Task PopulateLeaderboard()
        {
            var response = await Handlers.GetTopUsersHandler("global", 0, 10);

            List<(string username, int score)> leaderboardData = new();

            if (response != null && response.success && response.topUsers != null)
            {
                foreach (var entry in response.topUsers)
                {
                    leaderboardData.Add((entry.username, entry.score));
                }
            }
            else
            {
                Debug.LogWarning("Failed to load leaderboard data.");
            }

            View.PopulateLeaderboard(leaderboardData);
        }
    }
}
