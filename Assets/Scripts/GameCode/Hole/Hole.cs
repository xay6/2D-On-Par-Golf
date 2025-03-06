using UnityEngine;
using UnityEngine.SceneManagement;

public class Hole : MonoBehaviour
{
    private CollisionDetection collisionDetection;
    private GameObject player;

    public bool isGoal = false;

    int holeNumberlevel = 1;

    void Start()
    {
        collisionDetection = gameObject.GetComponent<CollisionDetection>();

        if (collisionDetection == null)
        {
            Debug.LogError("CollisionDetection component missing on Hole object!");
        }

        player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Ball not found! Make sure it has the correct tag.");
        }
    }

    void Update()
    {
        if (collisionDetection != null && player != null && collisionDetection.onSuperimposed)
        {
            player.GetComponent<SpriteRenderer>().enabled = false;

            if (isGoal)
            {
                Debug.Log("Goal reached! Loading next level...");
                LoadNextLevel();
            }
        }
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        ScoreManager.Instance.AddToOverallScore(ScoreManager.Instance.strokes);
        ScoreManager.Instance.ResetStrokes();

        holeNumberlevel++;
        if (HoleInfoManager.Instance != null)
        {
            HoleInfoManager.Instance.SetHoleNumber(holeNumberlevel);
        }
        else
        {
            Debug.LogError("HoleInfoManager Instance is NULL! Make sure it's in the scene.");
        }
        

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log($"Loading Level {nextSceneIndex}: {SceneManager.GetSceneByBuildIndex(nextSceneIndex).name}");
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more levels! Restarting from Level 1.");
            SceneManager.LoadScene(2); 
        }
    }
}
