using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Unity.Template.Multiplayer.NGO.Runtime
{
    internal class LeaderboardView : View<MetagameApplication>
    {
        Button m_BackButton;
        VisualElement m_Root;
        ScrollView m_LeaderboardList;
        UIDocument m_UIDocument;
        string currentUsername; // Set this when the player logs in

        void Awake()
        {
            m_UIDocument = GetComponent<UIDocument>();
            if (m_UIDocument != null)
            {
                m_Root = m_UIDocument.rootVisualElement;
            }
        }

        void OnEnable()
        {
            m_LeaderboardList = m_Root.Q<ScrollView>("leaderboardList");
            m_BackButton = m_Root.Q<Button>("backButton");
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

        internal void SetCurrentUsername(string username)
        {
            currentUsername = username;
        }

        internal void PopulateLeaderboard(List<(string username, int score)> leaderboardData)
        {
            m_LeaderboardList.Clear();

            if (leaderboardData == null || leaderboardData.Count == 0)
            {
                var emptyLabel = new Label("No leaderboard data available");
                emptyLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
                emptyLabel.style.fontSize = 20;
                m_LeaderboardList.Add(emptyLabel);
                return;
            }

            int rank = 1;
            foreach (var (username, score) in leaderboardData)
            {
                var row = new VisualElement();
                row.style.flexDirection = FlexDirection.Row;
                row.style.justifyContent = Justify.SpaceBetween;
                row.style.paddingTop = 4;
                row.style.paddingBottom = 4;
                row.style.paddingLeft = 10;
                row.style.paddingRight = 10;

                // Alternate row color
                if (rank % 2 == 0)
                    row.style.backgroundColor = new Color(0.95f, 1f, 0.95f);

                // Top 3 highlight
                if (rank == 1)
                    row.style.backgroundColor = new Color(1f, 0.95f, 0.6f); // gold
                else if (rank == 2)
                    row.style.backgroundColor = new Color(0.9f, 0.95f, 1f);  // silver
                else if (rank == 3)
                    row.style.backgroundColor = new Color(0.95f, 0.9f, 0.85f); // bronze

                var nameLabel = new Label($"#{rank}  {username}");
                var scoreLabel = new Label($"{score} pts");

                // Highlight current player
                if (username == currentUsername)
                {
                    nameLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
                    scoreLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
                }

                row.Add(nameLabel);
                row.Add(scoreLabel);
                m_LeaderboardList.Add(row);
                rank++;
            }
        }
    }
}