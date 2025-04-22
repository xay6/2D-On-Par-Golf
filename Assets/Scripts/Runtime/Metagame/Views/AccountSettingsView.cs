using UnityEngine;
using UnityEngine.UIElements;
using OnPar.RouterHandlers;
using OnPar.Routers;
using System.Linq;
// using System;

namespace Unity.Template.Multiplayer.NGO.Runtime
{
    internal class AccountSettingsView : View<MetagameApplication>
    {
        Label m_UsernameLabel;
        Label m_UnlockedLevelsLabel;
        Label m_BestScoresLabel;
        Label m_TotalStrokesLabel;
        Label m_AchievementsLabel;
        Button m_LogoutButton;
        Button m_BackButton;
        VisualElement m_Root;

        void OnEnable()
        {
            var uiDoc = GetComponent<UIDocument>();
            m_Root = uiDoc.rootVisualElement;

            m_UsernameLabel = m_Root.Q<Label>("usernameLabel");
            m_UnlockedLevelsLabel = m_Root.Q<Label>("unlockedLevelsLabel");
            m_BestScoresLabel = m_Root.Q<Label>("bestScoresLabel");
            m_TotalStrokesLabel = m_Root.Q<Label>("totalStrokesLabel");
            m_AchievementsLabel = m_Root.Q<Label>("achievementsLabel");
            m_LogoutButton = m_Root.Q<Button>("logoutButton");
            m_BackButton = m_Root.Q<Button>("backButton");

            m_LogoutButton.clicked += OnLogout;
            m_BackButton.clicked += OnBack;

            // Load data
            SetUsername(LoginRegister.getUsername());
            LoadUserStats();
        }

        internal void SetUsername(string username)
        {
            m_UsernameLabel.text = $"Username: {username}";
        }

        internal void SetProgress(int Maxlevel, string scores, int strokes, string achievements)
        {
            m_UnlockedLevelsLabel.text = (Maxlevel < 1) ? $"Unlocked Levels: 1" : $"Unlocked Levels: 1 - {Maxlevel}";
            m_BestScoresLabel.text = $"Best Scores: {scores}";
            m_TotalStrokesLabel.text = $"Total Strokes: {strokes}";
            m_AchievementsLabel.text = $"Achievements: {achievements}";
        }

        async void LoadUserStats()
        {
            string username = LoginRegister.getUsername();
            Debug.Log($"[AccountSettingsView] Fetching stats for username: {username}");

            int bestStrokes = int.MaxValue;
            string bestCourse = "N/A";
            int highestLevel = 0;
            int totalStrokes = 0;

            var allScores = await Handlers.GetAllUserScoresHandler(username);

            if (allScores != null && allScores.success && allScores.scores != null && allScores.scores.Count > 0)
            {
                foreach (var score in allScores.scores)
                {
                    Debug.Log(score.courseId.Substring(0, 5));
                    if (score.courseId.Substring(0, 5).Equals("Level"))
                    {
                        Debug.Log(score.courseId);
                        int levelNum = int.Parse(score.courseId.Substring(6));
                        if (levelNum > highestLevel)
                            highestLevel = levelNum;
                    

                        totalStrokes += score.score;

                        if (score.score < bestStrokes)
                        {
                            bestStrokes = score.score;
                            bestCourse = score.courseId;
                        }
                    }
                }
            }
            else
            {
                Debug.Log("No scores found for user.");
            }

            string bestScoreDisplay = bestStrokes != int.MaxValue ? $"{bestCourse} - {bestStrokes} strokes" : "N/A";

            // Load achievements
            string achievements = "None";
            var rewardsResponse = await Handlers.GetRewardsHandler(username);
            if (rewardsResponse != null && rewardsResponse.success && rewardsResponse.rewards.Length > 0)
            {
                achievements = string.Join(", ", rewardsResponse.rewards);
            }

            SetProgress(highestLevel, bestScoreDisplay, totalStrokes, achievements);
        }

        void OnLogout()
        {
            //bool confirm = EditorUtility.DisplayDialog("Confirm Logout", "Are you sure you want to log out?", "Yes", "No");
            //if (!confirm) return;

            //LoginRegister.ClearSession();
            Handlers.LogoutHandler();
            Broadcast(new ExitAccountSettingsEvent());
            Broadcast(new ExitAccountEvent());
        }

        void OnBack()
        {
            Debug.Log("Going back to AccountView...");
            Broadcast(new ExitAccountSettingsEvent());
        }
    }
}