using UnityEngine;

public class ApplyWind : MonoBehaviour
{
    private Rigidbody2D rb;
    private Wind wind;
    private bool isMoving = false;

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
                Vector2 windForce = wind.windDirection * (wind.windStrength * 0.04f); 
                rb.AddForce(windForce);

                Debug.Log($"Applying Wind: {wind.windStrength:F1} mph {wind.GetWindDirectionString(wind.windDirection)}");
                isMoving = true;
            }
            else if (isMoving) 
            {
                rb.linearVelocity = Vector2.zero;
                wind.ApplyNextWind(); 
                wind.GenerateNewWind(); 
                isMoving = false;
            }
        }
    }
}






