using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int strokes = 0;
    public int overallScore = 0;

    public TextMeshProUGUI scoreText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        FindScoreText();
        UpdateScoreText();
    }

    private void OnLevelWasLoaded(int level)
    {
        FindScoreText();
        UpdateScoreText();
    }

    void FindScoreText()
    {
        GameObject textObj = GameObject.FindWithTag("ScoreText");

        if (textObj != null)
        {
            scoreText = textObj.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogWarning("ScoreText not found! Make sure it's tagged properly in the scene.");
        }
    }
    public void AddStroke()
    {
        strokes++;
        UpdateScoreText();
    }

    public void AddToOverallScore(int score)
    {
        overallScore += strokes;
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
            scoreText.text = $"Strokes: {strokes} \nTotal Score: {overallScore}";
        }
    }
}