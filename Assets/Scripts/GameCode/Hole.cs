using UnityEngine;

public class Hole : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball")) 
        {
            Debug.Log("Ball entered the hole!");
            Destroy(other.gameObject); 
        }
    }
}
