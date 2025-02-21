using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int strokes = 0;
    public int overallScore = 0;

    public TextMeshProUGUI scoreText; // Reference to your TextMeshPro text
    
        private void Awake()
    {
        // Singleton pattern setup (Optional, but good if you want easy global access)
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateScoreText();
    }

    // This is your main stroke counter
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
            scoreText.text = $"Strokes: {strokes}\n Overall Score: {overallScore}";
        }
    }
}