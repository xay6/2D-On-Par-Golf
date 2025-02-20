using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Template.Multiplayer.NGO.Runtime
{
    internal class LeaderboardView : View<MetagameApplication>
    {
        Button m_BackButton;
        ScrollView m_LeaderboardList;
        UIDocument m_UIDocument;

        void Awake()
        {
            m_UIDocument = GetComponent<UIDocument>();
        }

        void Start()
        {
            Hide(); // Ensure leaderboard starts hidden
        }

        void OnEnable()
        {
            if (m_UIDocument == null)
            {
                Debug.LogError("LeaderboardView: UIDocument is missing!");
                return;
            }

            var root = m_UIDocument.rootVisualElement;
            if (root == null)
            {
                Debug.LogError("LeaderboardView: rootVisualElement is null!");
                return;
            }

            m_LeaderboardList = root.Q<ScrollView>("leaderboardList");
            m_BackButton = root.Q<Button>("backButton");

            if (m_BackButton == null || m_LeaderboardList == null)
            {
                Debug.LogError("LeaderboardView: UI elements not found!");
                return;
            }

            m_BackButton.RegisterCallback<ClickEvent>(OnClickBack);
        }

        void OnDisable()
        {
            if (m_BackButton != null)
            {
                m_BackButton.UnregisterCallback<ClickEvent>(OnClickBack);
            }
        }

        void OnClickBack(ClickEvent evt)
        {
            Hide();
            Broadcast(new ExitLeaderboardEvent());
        }

        internal void PopulateLeaderboard(List<(string username, int score)> leaderboardData)
        {
            if (m_LeaderboardList == null)
            {
                Debug.LogError("LeaderboardView: leaderboardList is null!");
                return;
            }

            m_LeaderboardList.Clear();

            if (leaderboardData == null || leaderboardData.Count == 0)
            {
                var emptyLabel = new Label("No leaderboard data available");
                m_LeaderboardList.Add(emptyLabel);
                return;
            }

            foreach (var (username, score) in leaderboardData)
            {
                var label = new Label($"{username}: {score} points");
                m_LeaderboardList.Add(label);
            }
        }
    }
}
