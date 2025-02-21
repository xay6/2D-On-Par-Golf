using UnityEngine;

public class Sand : MonoBehaviour
{
    private Rigidbody2D ballRB;
    private ClubStats currentClub;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            ballRB = other.GetComponent<Rigidbody2D>();
            currentClub = other.GetComponent<GolfClubController>().CurrentClub;
            if (ballRB != null && currentClub != null)
            {
                ballRB.linearDamping = 20f; 
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
            }
        }
    }
}
