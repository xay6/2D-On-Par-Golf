using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

namespace Unity.Template.Multiplayer.NGO.Runtime
{
    internal class MainMenuView : View<MetagameApplication>
    {
        internal Button NewGameButton { get; private set; } // Renamed to match UXML
        Button LoginButton;
        Button LeaderboardButton;
        Button QuitButton; // Added QuitButton
        Label TitleLabel;
        VisualElement Root;

        void OnEnable()
        {
            var uiDocument = GetComponent<UIDocument>();
            Root = uiDocument.rootVisualElement;

            // Match buttons with UXML names
            NewGameButton = Root.Q<Button>("newGameButton");
            NewGameButton.RegisterCallback<ClickEvent>(OnClickFindMatch);

            LoginButton = Root.Q<Button>("loginButton");
            LoginButton.RegisterCallback<ClickEvent>(OnClickStartSinglePlayer);

            LeaderboardButton = Root.Q<Button>("leadrboardButton");
            LeaderboardButton.RegisterCallback<ClickEvent>(OnClickLeaderboard);

            QuitButton = Root.Q<Button>("quitButton"); // Fixed Quit Button Reference
            QuitButton.RegisterCallback<ClickEvent>(OnClickQuit);

            // Update title label text
            TitleLabel = Root.Q<Label>("titleLabel");
            if (TitleLabel != null)
            {
                TitleLabel.text = "Game Title";
            }

            CustomNetworkManager.OnConfigurationLoaded += OnGameConfigurationLoaded;
        }

        void OnGameConfigurationLoaded()
        {
            DisableControlsUnsupportedInAutoconnectMode();
        }

        void OnDisable()
        {
            NewGameButton.UnregisterCallback<ClickEvent>(OnClickFindMatch);
            LoginButton.UnregisterCallback<ClickEvent>(OnClickStartSinglePlayer);
            LeaderboardButton.UnregisterCallback<ClickEvent>(OnClickLeaderboard);
            QuitButton.UnregisterCallback<ClickEvent>(OnClickQuit);

            CustomNetworkManager.OnConfigurationLoaded -= OnGameConfigurationLoaded;
        }

        void OnClickFindMatch(ClickEvent evt)
        {
            Broadcast(new EnterMatchmakerQueueEvent("Standard"));
        }

        void OnClickStartSinglePlayer(ClickEvent evt)
        {
            SceneManager.LoadScene("Level01");
        }

        void OnClickLeaderboard(ClickEvent evt)
        {
           // Debug.Log("Leaderboard Button Clicked!");
            // Add leaderboard logic here
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
            NewGameButton.SetEnabled(false);
            LoginButton.SetEnabled(false);
            LeaderboardButton.SetEnabled(false);
        }
    }
}
