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

        async void PopulateLeaderboard()
        {
            var leaderboardData = await LeaderboardService.FetchLeaderboard("global");
            View.PopulateLeaderboard(leaderboardData);
        }
    }
}
