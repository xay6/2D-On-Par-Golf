using UnityEngine;

public class ApplyWind : MonoBehaviour
{
    private Rigidbody2D rb;
    private Wind wind;
    private bool isMoving = false;
    private bool hasPlayedSound;

    private AudioSource audioSource;
    [SerializeField] private AudioClip windSound;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        wind = FindFirstObjectByType<Wind>();
        audioSource = GetComponent<AudioSource>();  
    }

    void FixedUpdate()
    {
        if (rb != null && wind != null)
        {
            if (rb.linearVelocity.magnitude > 0.5f) // Ball is moving
            {
                // Apply wind force based on direction and strength from the wind script
                Vector2 windForce = wind.windDirection * (wind.windStrength * 0.05f);
                rb.AddForce(windForce);

                Debug.Log($"Applying Wind: {wind.windStrength:F1} mph {wind.GetWindDirectionString(wind.windDirection)}");

                if (!hasPlayedSound)
                {
                    wind.PlayWindSound();
                    hasPlayedSound = true;  
                }
            }
            else if (isMoving) 
            {
                rb.linearVelocity = Vector2.zero;  
                wind.GenerateNewWind(); 
                isMoving = false;

                if (wind != null)
                {
                    wind.StopWindSound(); 
                }
            }
        }
    }
}
