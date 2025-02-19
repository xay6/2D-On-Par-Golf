using UnityEngine;
using UnityEngine.UI;

public class Wind : MonoBehaviour
{
    public Vector2 windDirection;
    public float windStrength;
    public RectTransform windArrow;

    void Start()
    {
        GenerateNewWind();
    }

    public void GenerateNewWind()
    {
        windDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        windStrength = Random.Range(0f, 3f);
        UpdateWindIndicator();
    }

    void UpdateWindIndicator()
    {
        if (windArrow != null)
        {
            float angle = Mathf.Atan2(windDirection.y, windDirection.x) * Mathf.Rad2Deg;
            windArrow.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}


