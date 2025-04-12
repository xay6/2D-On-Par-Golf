using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using OnPar.RouterHandlers;
using OnPar.Routers;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int strokes = 0;
    public int overallScore = 0;
    public static Message UpdateScoreResponse;

    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI scoreTextPostUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // No need to find UI here anymore — UI elements will register themselves!
        UpdateScoreText();
    }

    // Public method for UI to register itself
    public void RegisterScoreText(TextMeshProUGUI text)
    {
        scoreText = text;
        UpdateScoreText();
    }

    public void RegisterScoreTextPostUI(TextMeshProUGUI text)
    {
        scoreTextPostUI = text;
        UpdateScoreText();
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

    private async void UpdateScoresHelper(string username)
    {
        int score = strokes;
        string courseId = SceneManager.GetActiveScene().name;
        UpdateScoreResponse = await Handlers.AddOrUpdateScore(courseId, username, score);
    }
}
