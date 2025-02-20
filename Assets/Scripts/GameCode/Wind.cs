using UnityEngine;
using UnityEngine.UI;

public class Wind : MonoBehaviour
{
    public Vector2 windDirection { get; private set; }
    public float windStrength { get; private set; }
    public Text windDisplayText;

    void Start()
    {
        GenerateNewWind(true);
    }

    public void GenerateNewWind(bool firstTime = false)
    {
        if (firstTime)
        {
            // ✅ Generate a fully random wind direction on game start
            windDirection = GetRandomWindDirection();
            windStrength = Random.Range(1f, 5f);
        }
        else
        {
            // ✅ Ensure wind changes more randomly each time
            windDirection = GetRandomWindDirection();
            windStrength = Mathf.Clamp(windStrength + Random.Range(-1f, 1f), 1f, 5f);
        }

        UpdateWindUI();
    }

    void UpdateWindUI()
    {
        if (windDisplayText != null)
        {
            string directionText = GetWindDirectionString(windDirection);
            windDisplayText.text = $"Wind: {windStrength:F1} mph {directionText}";
        }

        Debug.Log($"Updated Wind UI: {windDisplayText.text}");
    }

    Vector2 GetRandomWindDirection()
    {
        // ✅ Generate a completely random wind direction between 0 and 360 degrees
        float randomAngle = Random.Range(0f, 360f);
        return new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad)).normalized;
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





