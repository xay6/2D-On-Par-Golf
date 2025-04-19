using UnityEngine;

public class Sand : MonoBehaviour
{
    private Rigidbody2D ballRB;
    private ClubStats currentClub;
    private LaunchWithDrag launchWithDrag;

    public bool ballEnteredSand { get; private set; }
    private float decelerationRate = 0.4f;

    private float originalDamping;
    private float originalForce;

    void Start()
    {
        ballEnteredSand = false;
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

                originalDamping = currentClub.linearDamping;
                originalForce = currentClub.forceMultiplier;

                ballRB.linearDamping = originalDamping * 3f;
                launchWithDrag.setForce(originalForce * 0.6f);

                ballRB.linearVelocity *= Mathf.Clamp01(1 - decelerationRate);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (ballEnteredSand && other.CompareTag("Player"))
        {
            ballRB.linearVelocity *= Mathf.Clamp01(1 - (decelerationRate * Time.deltaTime));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && ballEnteredSand)
        {
            if (ballRB != null && currentClub != null)
            {
                ballRB.linearDamping = originalDamping;
                launchWithDrag.setForce(originalForce);
                ballEnteredSand = false;
            }
        }
    }
}
