using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

namespace Unity.Template.Multiplayer.NGO.Runtime
{
    internal class MainMenuView : View<MetagameApplication>
    {
        internal Button NewGameButton { get; private set; }
        Button GuestButton;
        Button LoginButton;
        Button LeaderboardButton;
        Button QuitButton;
        Label TitleLabel;
        VisualElement Root;

        void OnEnable()
        {
            var uiDocument = GetComponent<UIDocument>();
            Root = uiDocument.rootVisualElement;

            // Get buttons
            //NewGameButton = Root.Q<Button>("newGameButton");
            //NewGameButton.RegisterCallback<ClickEvent>(OnClickNewGame);

            GuestButton = Root.Q<Button>("guestButton");
            GuestButton.RegisterCallback<ClickEvent>(OnClickGuest);

            LoginButton = Root.Q<Button>("loginButton");
            LoginButton.RegisterCallback<ClickEvent>(OnClickLogin);

            LeaderboardButton = Root.Q<Button>("leaderboardButton");
            LeaderboardButton.RegisterCallback<ClickEvent>(OnClickLeaderboard);

            QuitButton = Root.Q<Button>("quitButton");
            QuitButton.RegisterCallback<ClickEvent>(OnClickQuit);

            // Update title label
            TitleLabel = Root.Q<Label>("titleLabel");
            if (TitleLabel != null)
            {
                TitleLabel.text = "Golf Game";
            }

            CustomNetworkManager.OnConfigurationLoaded += OnGameConfigurationLoaded;
        }

        void OnGameConfigurationLoaded()
        {
            DisableControlsUnsupportedInAutoconnectMode();
        }

        void OnDisable()
        {
            //FindMatchButton.UnregisterCallback<ClickEvent>(OnClickFindMatch);
            //m_SinglePlayerButton.UnregisterCallback<ClickEvent>(OnClickStartSinglePlayer);
            GuestButton.UnregisterCallback<ClickEvent>(OnClickGuest);
            LoginButton.UnregisterCallback<ClickEvent>(OnClickLogin);
            LeaderboardButton.UnregisterCallback<ClickEvent>(OnClickLeaderboard); // Unregister Leaderboard
            QuitButton.UnregisterCallback<ClickEvent>(OnClickQuit);
            CustomNetworkManager.OnConfigurationLoaded -= OnGameConfigurationLoaded;
        }

        void OnClickFindMatch(ClickEvent evt)
        {
            Broadcast(new EnterMatchmakerQueueEvent("Standard"));
        }

        void OnClickStartSinglePlayer(ClickEvent evt)
        {
            Broadcast(new StartSinglePlayerModeEvent());
        }

        void OnClickLeaderboard(ClickEvent evt)
        {
            Broadcast(new EnterLeaderboardEvent()); // Broadcast leaderboard event
        }

        void OnClickLogin(ClickEvent evt)
        {
            Broadcast(new EnterLoginEvent()); // Broadcast leaderboard event
        }

        void OnClickGuest(ClickEvent evt)
        {
            Broadcast(new EnterGuestEvent());
        }

        void OnClickQuit(ClickEvent evt)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }

        internal void DisableControlsUnsupportedInAutoconnectMode()
        {
            if (!CustomNetworkManager.Singleton.AutoConnectOnStartup)
            {
                return;
            }
            //FindMatchButton.SetEnabled(false);
            //m_SinglePlayerButton.SetEnabled(false);
        }
    }
}
