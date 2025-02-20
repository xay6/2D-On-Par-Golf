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

        void OnEnable()
        {
            var root = m_UIDocument.rootVisualElement;
            m_LeaderboardList = root.Q<ScrollView>("leaderboardList");
            m_BackButton = root.Q<Button>("backButton");

            m_BackButton.RegisterCallback<ClickEvent>(OnClickBack);
        }

        void OnDisable()
        {
            m_BackButton.UnregisterCallback<ClickEvent>(OnClickBack);
        }

        void OnClickBack(ClickEvent evt)
        {
            Broadcast(new ExitLeaderboardEvent());
        }

        internal void PopulateLeaderboard(List<(string username, int score)> leaderboardData)
        {
            m_LeaderboardList.Clear();

            foreach (var entry in leaderboardData)
            {
                var label = new Label($"{entry.username}: {entry.score} points");
                m_LeaderboardList.Add(label);
            }
        }
    }
}
