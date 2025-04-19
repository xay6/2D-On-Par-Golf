using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using OnPar.RouterHandlers;
using OnPar.Routers;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int strokes = 0;
    private int maxStrokes = 3;
    public int overallScore = 0;
    public bool shouldTriggerGameOver = false;
    public static Message UpdateScoreResponse;

    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI scoreTextPostUI;
    private TextMeshProUGUI challengeStrokes;

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
    public void RegisterChallengeScoreTextPostUI(TextMeshProUGUI text)
    {
        challengeStrokes = text;
        UpdateScoreText();
    }

    public void AddStroke()
    {
        strokes++;
        UpdateScoreText();

        if (strokes >= maxStrokes){
            shouldTriggerGameOver = true;
        }
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
        string challengeScoreString = strokes + "/" + maxStrokes;

        if (scoreText != null)
        {
            scoreText.text = scoreString;
        }

        if (scoreTextPostUI != null)
        {
            scoreTextPostUI.text = scoreString;
        }

        if (challengeStrokes != null)
        {
            challengeStrokes.text = challengeScoreString;
        }
    }

    public async void UpdateScoresHelper(string username)
    {
        int score = strokes;
        string courseId = SceneManager.GetActiveScene().name;
        UpdateScoreResponse = await Handlers.AddOrUpdateScore(courseId, username, score);
    }
}
