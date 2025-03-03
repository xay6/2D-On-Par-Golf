using UnityEngine;

public class Sand : MonoBehaviour
{
    private Rigidbody2D ballRB;
    private ClubStats currentClub;
    private LaunchWithDrag launchWithDrag;
    public bool ballEnteredSand {set; get;}
    private float decelerationRate;

    void Start() 
    {
        ballEnteredSand = false;
        decelerationRate = 0.9f;
    }

    void Update() 
    {
        if(ballEnteredSand) {
            if(ballRB != null && currentClub != null)
            {
                ballRB.linearDamping = currentClub.linearDamping * 20f; 
                launchWithDrag.setForce(Mathf.Exp(currentClub.forceMultiplier / 10));
                ballRB.linearVelocity *= Mathf.Clamp01(1 - decelerationRate * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            ballRB = other.GetComponent<Rigidbody2D>();
            currentClub = other.GetComponent<GolfClubController>().CurrentClub;
            launchWithDrag = other.GetComponent<LaunchWithDrag>();
            if (ballRB != null && currentClub != null && launchWithDrag != null)
            {   
                ballEnteredSand = true;
                ballRB.linearVelocity *= Mathf.Clamp01(1 - decelerationRate * Time.deltaTime);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (ballRB != null && currentClub != null)
            {
                ballRB.linearDamping = currentClub.linearDamping; 
                launchWithDrag.setForce(currentClub.forceMultiplier);
                ballEnteredSand = false;
            }
        }
    }
}
