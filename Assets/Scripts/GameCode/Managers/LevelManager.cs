using JetBrains.Annotations;
using UnityEngine;
using OnPar.RouterHandlers;
using OnPar.Routers;

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
        if (LoginRegister.isLoggedIn()){
            
            Debug.Log(" Logged in, sending score...");
            AddScoreAsync(courseId, LoginRegister.getUsername(), ScoreManager.Instance.strokes);
        }
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

}
