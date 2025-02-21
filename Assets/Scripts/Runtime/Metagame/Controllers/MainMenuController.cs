using Unity.Properties;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity.Template.Multiplayer.NGO.Runtime
{
    internal class MainMenuController : Controller<MetagameApplication>
    {
        MainMenuView View => App.View.MainMenu;

        void Awake()
        {
            AddListener<EnterMatchmakerQueueEvent>(OnEnterMatchmakerQueue);
            AddListener<ExitMatchmakerQueueEvent>(OnExitMatchmakerQueue);
            AddListener<MatchLoadingEvent>(OnMatchLoading);
            AddListener<ExitedMatchmakerQueueEvent>(OnExitedMatchmakerQueue);
            AddListener<StartSinglePlayerModeEvent>(OnStartSinglePlayerMode);
            AddListener<EnterLeaderboardEvent>(OnEnterLeaderboard);
            AddListener<ExitLeaderboardEvent>(OnExitLeaderboard);
            AddListener<EnterLoginEvent>(OnEnterLogin);
            AddListener<ExitLoginEvent>(OnExitLogin);
        }

        void OnDestroy()
        {
            RemoveListeners();
        }

        internal override void RemoveListeners()
        {
            RemoveListener<MatchLoadingEvent>(OnMatchLoading);
            RemoveListener<EnterMatchmakerQueueEvent>(OnEnterMatchmakerQueue);
            RemoveListener<ExitMatchmakerQueueEvent>(OnExitMatchmakerQueue);
            RemoveListener<ExitedMatchmakerQueueEvent>(OnExitedMatchmakerQueue);
            RemoveListener<StartSinglePlayerModeEvent>(OnStartSinglePlayerMode);
            RemoveListener<EnterLeaderboardEvent>(OnEnterLeaderboard);
            RemoveListener<ExitLeaderboardEvent>(OnExitLeaderboard);
            RemoveListener<EnterLoginEvent>(OnEnterLogin);
            RemoveListener<ExitLoginEvent>(OnExitLogin);
        }

        void OnMatchLoading(MatchLoadingEvent evt)
        {
            if (!CustomNetworkManager.Singleton.AutoConnectOnStartup)
            {
                return;
            }
            View.Hide();
            App.View.LoadingScreen.Show();
        }

        void OnEnterMatchmakerQueue(EnterMatchmakerQueueEvent evt)
        {
            View.Hide();
        }

        void OnExitMatchmakerQueue(ExitMatchmakerQueueEvent evt)
        {
            View.Show();
            View.NewGameButton.SetEnabled(false);
        }

        void OnExitedMatchmakerQueue(ExitedMatchmakerQueueEvent evt)
        {
            View.NewGameButton.SetEnabled(true);
        }

        void OnStartSinglePlayerMode(StartSinglePlayerModeEvent evt)
        {
            View.Hide();
            CustomNetworkManager.Singleton.InitializeNetworkLogic(true, false);
        }

        void OnEnterLeaderboard(EnterLeaderboardEvent evt)
        {
            View.Hide();
            App.View.Leaderboard.Show();
        }

        void OnExitLeaderboard(ExitLeaderboardEvent evt)
        {
            App.View.Leaderboard.Hide();
            View.Show();
        }

        void OnEnterLogin(EnterLoginEvent evt)
        {
            View.Hide();
            App.View.Login.Show();
        }
        void OnExitLogin(ExitLoginEvent evt)
        {
            App.View.Login.Hide();
            View.Show();
        }
    }
}