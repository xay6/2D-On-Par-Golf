using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class LeaderboardView : MonoBehaviour
{
    public event System.Action OnBack;

    private ScrollView leaderboardList;
    private Button backButton;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        leaderboardList = root.Q<ScrollView>("leaderboardList");
        backButton = root.Q<Button>("backButton");

        backButton.clicked += () => OnBack?.Invoke();
    }

    public void PopulateLeaderboard(List<(string username, int score)> leaderboardData)
    {
        leaderboardList.Clear();
        foreach (var entry in leaderboardData)
        {
            var label = new Label($"{entry.username}: {entry.score} points");
            leaderboardList.Add(label);
        }
    }
}
