using JetBrains.Annotations;
using UnityEngine;
using OnPar.RouterHandlers;
using OnPar.Routers;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;
    public static Message UpdateScoreResponse;
    string courseId;
    
    [SerializeField] private GameObject levelCompleteUI;
    [SerializeField] private GameObject GameOverUI;
    [HideInInspector] public bool levelCompleted;


     void Awake()
    {
        main = this;
    }

    public void LevelComplete()
    {
        levelCompleted = true;
        courseId = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

   
    if (ScoreManager.Instance != null)
    {
        int strokesThisLevel = ScoreManager.Instance.strokes;
        ScoreManager.Instance.AddToOverallScore(ScoreManager.Instance.strokes);
        ScoreManager.Instance.shouldTriggerGameOver = false;
        if (LoginRegister.isLoggedIn()){
            
            Debug.Log(" Logged in, sending score...");
            AddScoreAsync(courseId, LoginRegister.getUsername(), ScoreManager.Instance.strokes);
        }
    }

    if (IsFinalChallengeLevel())
    {
        RewardPlayer();
    }

    levelCompleteUI.SetActive(true);
    
    }
    public async void  AddScoreAsync( string courseId, string username, int score)
    {
        UpdateScoreResponse = await Handlers.AddOrUpdateScore(courseId, username, score);
    }

    public void GameOver(){
        GameOverUI.SetActive(true);
    }

    private bool IsFinalChallengeLevel()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        return sceneName == "ChallengeLevel6"; // Change this to your last level name
    }

    private void RewardPlayer()
    {
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.AddCoins(1000); // or however many you want
            Debug.Log("🎉 Player rewarded with coins for completing all challenge levels!");
        }
    }


}
