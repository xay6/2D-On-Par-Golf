using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerMeterUI : MonoBehaviour
{
    public Image powerBarFill; // The bar that fills up
    public TextMeshProUGUI powerPercentageText; // Optional percentage text

    private float maxDragDistance = 5f; // Adjust as needed
    private CanvasGroup canvasGroup;

    void Start()
    {
        // Get CanvasGroup and start hidden
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
        }

        // Reset visuals
        powerBarFill.fillAmount = 0f;
        if (powerPercentageText != null)
            powerPercentageText.text = "0%";
    }

    public void ShowPowerMeter()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
        }
    }

    public void HidePowerMeter()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
        }
    }

    public void UpdatePowerMeter(float dragDistance)
    {
        float powerPercentage = Mathf.Clamp01(dragDistance / maxDragDistance);
        powerBarFill.fillAmount = powerPercentage;

        if (powerPercentageText != null)
        {
            powerPercentageText.text = Mathf.RoundToInt(powerPercentage * 100) + "%";
        }
    }
}
