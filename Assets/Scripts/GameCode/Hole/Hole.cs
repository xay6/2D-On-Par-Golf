
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.SceneManagement;

public class Hole : MonoBehaviour
{
    private CollisionDetection collisionDetection;
    private GameObject player;

    public bool isGoal = false; 

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
                StartCoroutine(LoadNextLevel(2f));
            }
        }
    }

    IEnumerator LoadNextLevel(float delay)
    {
        yield return new WaitForSeconds(delay);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        ScoreManager.Instance.AddToOverallScore(ScoreManager.Instance.strokes);
        ScoreManager.Instance.ResetStrokes();
        

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
