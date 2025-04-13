using UnityEngine;
using TMPro;

public class ManualTestRunner : MonoBehaviour
{
    public PowerMeterUI powerMeter;
    public Wind wind;

    void Start()
    {
        RunPowerMeterTest();
        RunCitySelectionTest();
        RunWindStrengthTest();
    }

    void RunPowerMeterTest()
    {
        powerMeter.ShowPowerMeter();
        bool isVisible = powerMeter.GetComponent<CanvasGroup>().alpha == 1f;

        Debug.Log("PowerMeter Test - Should Be Visible: " + (isVisible ? "PASS ✅" : "FAIL ❌"));
    }

    void RunCitySelectionTest()
    {
        string[] validCities = { "New York", "London", "Tokyo", "Paris", "Sydney", "Berlin", "Moscow", "Los Angeles" };
        string currentText = wind.windDisplayText != null ? wind.windDisplayText.text : "";

        bool found = false;
        foreach (string city in validCities)
        {
            if (currentText.Contains(city))
            {
                found = true;
                break;
            }
        }

        Debug.Log("Wind City Test - Valid Random City: " + (found ? "PASS ✅" : "FAIL ❌"));
    }

    void RunWindStrengthTest()
    {
        float strength = wind.windStrength;
        bool valid = strength > 0f && strength < 150f;

        Debug.Log("Wind Strength Test - Strength = " + strength + " → " + (valid ? "PASS ✅" : "FAIL ❌"));
    }
}
