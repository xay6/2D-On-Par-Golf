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
    }

    void FixedUpdate()
    {
        if (rb != null && wind != null)
        {
            if (rb.linearVelocity.magnitude > 0.5f)
            {
                Vector2 windForce = wind.windDirection * (wind.windStrength * 0.05f);
                rb.AddForce(windForce);

                Debug.Log($"Applying Wind: {wind.windStrength:F1} mph {wind.GetWindDirectionString(wind.windDirection)}");
                isMoving = true;

                if (!isMoving)
                {
                    isMoving = true;

                    if (wind != null)
                    {
                        Debug.Log("✅ Calling PlayWindSound() in Wind.cs");
                        wind.PlayWindSound();
                    }
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







