using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Unity.Template.Multiplayer.NGO.Runtime
{
    internal class MainMenuView : View<MetagameApplication>
    {
        internal Button NewGameButton { get; private set; }
        Button LoginButton;
        Button LeaderboardButton;
        Button QuitButton;
        Label TitleLabel;
        VisualElement Root;

        void OnEnable()
        {
            var uiDocument = GetComponent<UIDocument>();
            Root = uiDocument.rootVisualElement;

            // Assign buttons
            NewGameButton = Root.Q<Button>("newGameButton");
            NewGameButton.RegisterCallback<ClickEvent>(OnClickNewGame);

            LoginButton = Root.Q<Button>("loginButton");
            LoginButton.RegisterCallback<ClickEvent>(OnClickLogin);

            LeaderboardButton = Root.Q<Button>("leaderboardButton");
            LeaderboardButton.RegisterCallback<ClickEvent>(OnClickLeaderboard);

            QuitButton = Root.Q<Button>("quitButton");
            QuitButton.RegisterCallback<ClickEvent>(OnClickQuit);
        }

        void OnDisable()
        {
            NewGameButton.UnregisterCallback<ClickEvent>(OnClickNewGame);
            LoginButton.UnregisterCallback<ClickEvent>(OnClickLogin);
            LeaderboardButton.UnregisterCallback<ClickEvent>(OnClickLeaderboard);
            QuitButton.UnregisterCallback<ClickEvent>(OnClickQuit);
        }

        void OnClickNewGame(ClickEvent evt)
        {
            SceneManager.LoadScene("SignUpScene");
        }

        void OnClickLogin(ClickEvent evt)
        {
            SceneManager.LoadScene("LoginScene");
        }

        void OnClickLeaderboard(ClickEvent evt)
        {
            SceneManager.LoadScene("LeaderboardScene");
        }

        void OnClickQuit(ClickEvent evt)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }
    }
}