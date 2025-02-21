using UnityEngine.UIElements;

namespace Unity.Template.Multiplayer.NGO.Runtime
{
    internal class LeaderboardView : View<MetagameApplication>
    {
        Button m_BackButton;
        VisualElement m_Root;
        ScrollView m_LeaderboardList;
        UIDocument m_UIDocument;

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
            m_BackButton =  m_Root.Q<Button>("backButton");
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

        internal void PopulateLeaderboard(System.Collections.Generic.List<(string username, int score)> leaderboardData)
        {

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
