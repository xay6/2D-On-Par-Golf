using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            View.Show();
            PopulateLeaderboard();
        }

        void OnExitLeaderboard(ExitLeaderboardEvent evt)
        {
            View.Hide();
            SceneManager.LoadScene("MetagameScene");
        }

        void PopulateLeaderboard()
        {
            List<(string username, int score)> mockLeaderboard = new()
            {
                ("Player1", 120),
                ("Player2", 100),
                ("Player3", 80),
                ("Player4", 60),
                ("Player5", 40),
            };

            View.PopulateLeaderboard(mockLeaderboard);
        }
    }
}
