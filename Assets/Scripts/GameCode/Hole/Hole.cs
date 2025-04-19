
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.SceneManagement;

public class Hole : MonoBehaviour
{
    private CollisionDetection collisionDetection;
    private GameObject player;
    public bool isGoal = false;
    private ClickAndDrag clickAndDrag;
    private LaunchWithDrag launchWithDrag;
    private LineRendererController lineRendererController;
    private bool goalProcessed = false;
 

    void Start(){
    
    player = GameObject.FindWithTag("Player");
    if (player == null)
    {
        Debug.LogError("Ball not found! Make sure it has the correct tag.");
    }

    collisionDetection = gameObject.GetComponent<CollisionDetection>();
    if (collisionDetection == null)
    {
        Debug.LogError("CollisionDetection component missing on Hole object!");
    }

    clickAndDrag = player.GetComponent<ClickAndDrag>();
    launchWithDrag = player.GetComponent<LaunchWithDrag>();
    lineRendererController = player.GetComponent<LineRendererController>();

    if (clickAndDrag == null || launchWithDrag == null || lineRendererController == null)
    {
        Debug.LogError("One or more player control scripts are missing!");
    }
    }

    void Update()
    {
        if (!goalProcessed && collisionDetection != null && player != null && collisionDetection.onSuperimposed)
        {
            goalProcessed = true; 

            player.GetComponent<SpriteRenderer>().enabled = false;

            if (clickAndDrag != null) clickAndDrag.enabled = false;
            if (launchWithDrag != null) launchWithDrag.enabled = false;
            if (lineRendererController != null) lineRendererController.enabled = false;

            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null){
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            if (isGoal)
            {
                Debug.Log("Goal reached! Loading next level...");

                LevelManager.main.LevelComplete();
            }
        }
    }



}
