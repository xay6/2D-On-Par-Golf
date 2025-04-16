using UnityEngine;
using UnityEngine.UIElements;
using OnPar.RouterHandlers;
using OnPar.Routers;

namespace Unity.Template.Multiplayer.NGO.Runtime
{
    internal class AccountSettingsView : View<MetagameApplication>
    {
        Label m_UsernameLabel;
        Toggle m_MusicToggle;
        Toggle m_SoundToggle;
        Label m_UnlockedLevelsLabel;
        Label m_BestScoresLabel;
        Label m_TotalStrokesLabel;
        Label m_AchievementsLabel;
        Button m_LogoutButton;
        Button m_BackButton;
        VisualElement m_Root;

        void Awake()
        {
            var uiDoc = GetComponent<UIDocument>();
            m_Root = uiDoc.rootVisualElement;

            m_UsernameLabel = m_Root.Q<Label>("usernameLabel");
            m_MusicToggle = m_Root.Q<Toggle>("musicToggle");
            m_SoundToggle = m_Root.Q<Toggle>("soundToggle");
            m_UnlockedLevelsLabel = m_Root.Q<Label>("unlockedLevelsLabel");
            m_BestScoresLabel = m_Root.Q<Label>("bestScoresLabel");
            m_TotalStrokesLabel = m_Root.Q<Label>("totalStrokesLabel");
            m_AchievementsLabel = m_Root.Q<Label>("achievementsLabel");
            m_LogoutButton = m_Root.Q<Button>("logoutButton");
            m_BackButton = m_Root.Q<Button>("backButton");

            // Register toggle events
            // m_MusicToggle.RegisterValueChangedCallback(evt => ToggleMusic(evt.newValue));
            // m_SoundToggle.RegisterValueChangedCallback(evt => ToggleSound(evt.newValue));

            m_LogoutButton.clicked += OnLogout;
            m_BackButton.clicked += OnBack;

            // Sync toggle state with saved volume
            float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
            bool isVolumeOn = savedVolume > 0.01f;
            m_MusicToggle.value = isVolumeOn;
            m_SoundToggle.value = isVolumeOn;

            // Load data
            LoadUserStats("Level1"); // Replace with dynamic courseId if needed
        }

        internal void SetUsername(string username)
        {
            m_UsernameLabel.text = $"Username: {username}";
        }

        internal void SetProgress(string levels, string scores, int strokes, string achievements)
        {
            m_UnlockedLevelsLabel.text = $"Unlocked Levels: {levels}";
            m_BestScoresLabel.text = $"Best Scores: {scores}";
            m_TotalStrokesLabel.text = $"Total Strokes: {strokes}";
            m_AchievementsLabel.text = $"Achievements: {achievements}";
        }

        async void LoadUserStats(string courseId)
        {
            string username = LoginRegister.getUsername();
            SetUsername(username);

            ScoresResponse scoreResponse = await Handlers.GetScoresHandler(courseId, username);

            if (scoreResponse != null && scoreResponse.success)
            {
                int strokes = scoreResponse.userScore.score;
                string bestScore = $"{courseId} - {strokes} strokes";
                SetProgress($"Only {courseId}", bestScore, strokes, "Hole in One Club");
            }
            else
            {
                Debug.LogWarning("Could not load user score");
                SetProgress("N/A", "N/A", 0, "None");
            }
        }

        // void ToggleMusic(bool isOn)
        // {
        //     float volume = isOn ? 1f : 0f;
        //     global::SettingsManager.Instance.SetVolume(volume);
        // }

        // void ToggleSound(bool isOn)
        // {
        //     float volume = isOn ? 1f : 0f;
        //     global::SettingsManager.Instance.SetVolume(volume);
        // }

        void OnLogout()
        {
            Debug.Log("Logging out...");
            // TODO: Clear session, go back to login view
            Broadcast(new ExitAccountSettingsEvent());
        }

        void OnBack()
        {
            Debug.Log("Going back to AccountView...");
            Broadcast(new ExitAccountSettingsEvent());
        }
    }
}