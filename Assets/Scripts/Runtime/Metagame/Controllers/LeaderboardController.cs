using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LeaderboardController : MonoBehaviour
{
    [SerializeField] private LeaderboardView leaderboardView;

    private void Awake()
    {
        leaderboardView.OnBack += HandleBack;
        PopulateLeaderboard();
    }

    private void HandleBack()
    {
        SceneManager.LoadScene("MetagameScene");
    }

    private void PopulateLeaderboard()
    {
        List<(string username, int score)> mockLeaderboard = new List<(string, int)>
        {
            ("Player1", 120),
            ("Player2", 100),
            ("Player3", 80),
            ("Player4", 60),
            ("Player5", 40)
        };

        leaderboardView.PopulateLeaderboard(mockLeaderboard);
    }
}
