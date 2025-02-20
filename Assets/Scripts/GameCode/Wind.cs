using UnityEngine;
using UnityEngine.UI;

public class Wind : MonoBehaviour
{
    public Vector2 windDirection { get; private set; }
    public float windStrength { get; private set; }
    public Text windDisplayText; 

    private Vector2 upcomingWindDirection;
    private float upcomingWindStrength;

    void Start()
    {
        GenerateNewWind(true); 
        ApplyNextWind();
    }

    public void GenerateNewWind(bool firstTime = false)
    {
        if (firstTime)
        {
            windDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            windStrength = Random.Range(1f, 5f);
        }
        else
        {
            float angleChange = Random.Range(-15f, 15f);
            float newAngle = Mathf.Atan2(windDirection.y, windDirection.x) * Mathf.Rad2Deg + angleChange;
            upcomingWindDirection = new Vector2(Mathf.Cos(newAngle * Mathf.Deg2Rad), Mathf.Sin(newAngle * Mathf.Deg2Rad)).normalized;

            upcomingWindStrength = Mathf.Clamp(windStrength + Random.Range(-1f, 1f), 1f, 5f);
        }

        UpdateWindUI(true); 
    }

    public void ApplyNextWind()
    {
        windDirection = upcomingWindDirection;
        windStrength = upcomingWindStrength;
        UpdateWindUI(false); 
    }

    void UpdateWindUI(bool showUpcoming = false)
    {
        if (windDisplayText != null)
        {
            if (showUpcoming)
            {
                string directionText = GetWindDirectionString(upcomingWindDirection);
                windDisplayText.text = $"Next Wind: {upcomingWindStrength:F1} mph {directionText}";
            }
            else
            {
                string directionText = GetWindDirectionString(windDirection);
                windDisplayText.text = $"Wind: {windStrength:F1} mph {directionText}";
            }

            Debug.Log($"Updated Wind UI: {windDisplayText.text}");
        }
    }

    public string GetWindDirectionString(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;

        if (angle >= 337.5 || angle < 22.5) return "→ East";
        if (angle >= 22.5 && angle < 67.5) return "↗ NE";
        if (angle >= 67.5 && angle < 112.5) return "↑ North";
        if (angle >= 112.5 && angle < 157.5) return "↖ NW";
        if (angle >= 157.5 && angle < 202.5) return "← West";
        if (angle >= 202.5 && angle < 247.5) return "↙ SW";
        if (angle >= 247.5 && angle < 292.5) return "↓ South";
        return "↘ SE";
    }
}




