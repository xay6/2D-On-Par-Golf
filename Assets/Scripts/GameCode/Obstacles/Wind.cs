using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;

public class Wind : MonoBehaviour
{
    public Vector2 windDirection { get; private set; }
    public float windStrength { get; private set; }
    public TextMeshProUGUI windDisplayText; 
    private AudioSource audioSource;
    [SerializeField] private AudioClip windSound;

    private bool isPlayingWindSound;
    private string apiKey = "b87062fe083e85134dddad7c9a8271b5"; 
    private string city; 

    private string[] cities = new string[] { "New York", "London", "Tokyo", "Paris", "Sydney", "Berlin", "Moscow", "Los Angeles" };

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        GameObject textObj = GameObject.FindWithTag("WindText");
        if (textObj != null)
        {
            windDisplayText = textObj.GetComponent<TextMeshProUGUI>();
        }

        audioSource.loop = true; 
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0; 
        audioSource.volume = 0.7f;

        GenerateRandomCity(); 
        FetchWindData(); 

        
    }

    private void GenerateRandomCity()
    {
        city = cities[Random.Range(0, cities.Length)];
        Debug.Log($"Randomly selected city: {city}");
    }

    // Fetch real-time wind data from OpenWeatherMap API
    public void FetchWindData()
    {
        string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=imperial"; 

        StartCoroutine(RequestWindData(url)); 
    }

    private IEnumerator RequestWindData(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            ParseWindData(jsonResponse); // Parse the API response
        }
        else
        {
            Debug.LogError("Error fetching wind data: " + request.error);
        }
    }

    // Parse the wind data from the API response
    private void ParseWindData(string jsonResponse)
    {
        var json = JsonUtility.FromJson<WeatherResponse>(jsonResponse);
        
        // Set the wind direction and strength based on the API response
        windDirection = new Vector2(Mathf.Cos(json.wind.deg * Mathf.Deg2Rad), Mathf.Sin(json.wind.deg * Mathf.Deg2Rad)).normalized;
        windStrength = json.wind.speed;

        UpdateWindUI(); // Update the UI with the new wind data
    }

    // Update UI with the fetched wind data
    void UpdateWindUI()
    {
        if (windDisplayText != null)
        {
            string directionText = GetWindDirectionString(windDirection);
            windDisplayText.text = $"Wind: {windStrength:F1} mph {directionText} in {city}";
        }

        Debug.Log($"Updated Wind UI: {windDisplayText.text}");
    }

    public string GetWindDirectionString(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;

        // Wind direction strings based on angle
        if (angle >= 337.5 || angle < 22.5) return "→ East";
        if (angle >= 22.5 && angle < 67.5) return "↗ NE";
        if (angle >= 67.5 && angle < 112.5) return "↑ North";
        if (angle >= 112.5 && angle < 157.5) return "↖ NW";
        if (angle >= 157.5 && angle < 202.5) return "← West";
        if (angle >= 202.5 && angle < 247.5) return "↙ SW";
        if (angle >= 247.5 && angle < 292.5) return "↓ South";
        return "↘ SE";
    }

    // Data class to parse the response from OpenWeatherMap
    [System.Serializable]
    public class WeatherResponse
    {
        public WindData wind;
    }

    [System.Serializable]
    public class WindData
    {
        public float speed;
        public float deg;
    }

    // Generate random wind (Fallback in case you want to update wind without using API)
    public void GenerateNewWind(bool firstTime = false)
    {
        if (firstTime)
        {
            // Generate random wind direction and strength for the first time
            windDirection = GetRandomWindDirection();
            windStrength = Random.Range(1f, 5f);
        }
        else
        {
            // Adjust wind direction and strength after the initial generation
            windDirection = GetRandomWindDirection();
            windStrength = Mathf.Clamp(windStrength + Random.Range(-1f, 1f), 1f, 5f);
        }

        UpdateWindUI(); // Update the UI with the new wind data
    }

    Vector2 GetRandomWindDirection()
    {
        // Generate random wind direction (0 to 360 degrees)
        float randomAngle = Random.Range(0f, 360f);
        return new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad)).normalized;
    }

    public void PlayWindSound()
    {
        if (windSound == null)
        {
            Debug.LogError("ERROR: Wind AudioClip is NULL! Assign it in the Inspector.");
            return;
        }

        if (SoundFXManager.instance != null && !isPlayingWindSound)
        {
            Debug.Log("✅ Playing Wind Sound...");
            SoundFXManager.instance.PlaySoundEffect(windSound, transform, 0.7f);
            isPlayingWindSound = true; 
        }
    }

    public void StopWindSound()
    {
        isPlayingWindSound = false; 
    }
}
