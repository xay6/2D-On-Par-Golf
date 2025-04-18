using JetBrains.Annotations;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    [SerializeField] private GameObject levelCompleteUI;
    [SerializeField] private GameObject GameOverUI;
    [HideInInspector] public bool levelCompleted;

     void Awake()
    {
        main = this;
    }

    public void LevelComplete(){
    levelCompleted = true;

    // ✅ Add the current strokes to the total score
    if (ScoreManager.Instance != null)
    {
        ScoreManager.Instance.AddToOverallScore(ScoreManager.Instance.strokes);
        ScoreManager.Instance.shouldTriggerGameOver = false;
    }

    levelCompleteUI.SetActive(true);
}

    public void GameOver(){
        GameOverUI.SetActive(true);
    }

}
