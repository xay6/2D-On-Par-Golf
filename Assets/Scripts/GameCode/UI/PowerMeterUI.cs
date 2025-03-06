using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class PowerMeterUI : MonoBehaviour
{
    public Image powerBarFill; 
    public TextMeshProUGUI powerPercentageText; 

    private float maxDragDistance = 5f; 

    void Start()
    {
        gameObject.SetActive(false); 
    }

    public void ShowPowerMeter()
    {
        gameObject.SetActive(true); 
    }

    public void HidePowerMeter()
    {
        gameObject.SetActive(false); 
    }

    public void UpdatePowerMeter(float dragDistance)
    {
        float powerPercentage = Mathf.Clamp01(dragDistance / maxDragDistance); 

        powerBarFill.fillAmount = powerPercentage;
        powerPercentageText.text = Mathf.RoundToInt(powerPercentage * 100) + "%";
    }
}
