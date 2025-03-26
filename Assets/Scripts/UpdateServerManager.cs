using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using OnPar.RouterHandlers;
using OnPar.Routers;

public class UpdateServerManager : MonoBehaviour
{
    public static UpdateServerManager Instance;
    public static Message UpdateScoreResponse;

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

    void OnEnable()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    void OnSceneUnloaded(Scene scene)
    {
        // This method is called when a scene is about to unload
        if(scene.name.Contains("Level")) {
            Debug.Log(LoginRegister.getUsername());
            UpdateScoresHelper(LoginRegister.getUsername());
        }
    }

    private async void UpdateScoresHelper(string username) {
        int score = ScoreManager.Instance.strokes;
        string courseId = SceneManager.GetActiveScene().name;
        UpdateScoreResponse = await Handlers.AddOrUpdateScore(courseId, username, score);
        Debug.LogWarning(score);
    }
}
