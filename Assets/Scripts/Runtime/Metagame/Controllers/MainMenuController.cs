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

        // New functions to navigate to different scenes
        public void StartNewGame()
        {
            SceneManager.LoadScene("SignUpScene");
        }

        public void Login()
        {
            SceneManager.LoadScene("LoginScene");
        }

        public void ShowLeaderboard()
        {
            SceneManager.LoadScene("LeaderboardScene");
        }
    }
}