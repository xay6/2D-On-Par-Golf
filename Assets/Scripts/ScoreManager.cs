using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int strokes = 0;
    public int overallScore = 0;

    public TextMeshProUGUI scoreText; // Reference to your TextMeshPro text


    private void Start()
    {
        UpdateScoreText();
    }

    public void AddStroke()
    {
        strokes++;
        UpdateScoreText();
    }

    public void AddToOverallScore(int score)
    {
        overallScore += score;
        UpdateScoreText();
    }

    public void ResetStrokes()
    {
        strokes = 0;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"strokes: {strokes}\noverall score: {overallScore}";
        }
    }
}