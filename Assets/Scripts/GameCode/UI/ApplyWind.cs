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
            if (rb.linearVelocity.magnitude > 0.3f) // Ball is moving
            {
                
                float adjustedStrength = Mathf.Clamp(wind.windStrength, 7f, 18f);
                float gust = Random.Range(0.8f, 1.2f); 
                Vector2 windForce = wind.windDirection * adjustedStrength * gust * 0.12f;

                rb.AddForce(windForce);

                Debug.Log($"💨 Wind Applied: {adjustedStrength:F1} mph | Force: {windForce} | Gust: {gust:F2}");

                if (!hasPlayedSound)
                {
                    wind.PlayWindSound();
                    hasPlayedSound = true;  
                }

                isMoving = true; // Mark ball as moving
            }
            else if (isMoving)
            {
                rb.linearVelocity = Vector2.zero;
                isMoving = false;

                wind.StopWindSound();
                hasPlayedSound = false;
            }
        }
    }
}
