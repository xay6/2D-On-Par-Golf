using UnityEngine;

public class Sand : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball")) 
        {
            Rigidbody2D ballRB = other.GetComponent<Rigidbody2D>();
            if (ballRB != null)
            {
                ballRB.linearDamping = 3f; 
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            Rigidbody2D ballRB = other.GetComponent<Rigidbody2D>();
            if (ballRB != null)
            {
                ballRB.linearDamping = 0.5f; 
            }
        }
    }
}
