using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using OnPar.RouterHandlers;
using OnPar.Routers;
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int strokes = 0;
    public int overallScore = 0;
    public static Message UpdateScoreResponse;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreTextPostUI;

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
        GameObject postUITextobj = GameObject.FindWithTag("ScoreTextPostUI");

        if (textObj != null)
        {
            scoreText = textObj.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogWarning("ScoreText not found! Make sure it's tagged properly in the scene.");
        }

        if (postUITextobj != null)
        {
            scoreTextPostUI = postUITextobj.GetComponent<TextMeshProUGUI>();
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
        UpdateScoresHelper(LoginRegister.getUsername());
        FindScoreText();
        strokes = 0;
        UpdateScoreText();
        
    }

    private void UpdateScoreText()
    {
        //scoreText.GetScoresHandler(string courseId, string username);

        string scoreString = $"Strokes: {strokes} \nTotal Score: {overallScore}";

        if (scoreText != null)
        {
            scoreText.text = scoreString;
        }

        if (scoreTextPostUI != null)
        {
            scoreTextPostUI.text = scoreString;
        }
    }
    private async void UpdateScoresHelper(string username) {
        int score = ScoreManager.Instance.strokes;
        string courseId = SceneManager.GetActiveScene().name;
        UpdateScoreResponse = await Handlers.AddOrUpdateScore(courseId, username, score);
    }
}