using UnityEngine;
using OnPar.RouterHandlers;
using OnPar.Routers;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;
    public static Message UpdateScoreResponse;
    
    [SerializeField] private GameObject levelCompleteUI;
    [HideInInspector] public bool levelCompleted;

     void Awake()
    {
        main = this;
    }

    public void LevelComplete(string )
    {
        levelCompleted = true;

    // ✅ Add the current strokes to the total score
    if (ScoreManager.Instance != null)
    {
        ScoreManager.Instance.AddToOverallScore(ScoreManager.Instance.strokes);
        AddScore(courseId,LoginRegister.getUsername(),ScoreManager.Instance.strokes);
    }

        levelCompleteUI.SetActive(true);
    }
    public async void  AddScore( string courseId, string username, int score)
    {
        UpdateScoreResponse = await Handlers.AddOrUpdateScore(courseId, username, score);
    }

}
