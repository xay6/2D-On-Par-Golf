using UnityEngine;
using TMPro;

public class HoleInfoManager : MonoBehaviour
{
    public static HoleInfoManager Instance;

    public TextMeshProUGUI holeText;
    private int currentHole = 1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateHoleInfo();
    }

    public void IncrementHole()
    {
        currentHole++;
        UpdateHoleInfo();
    }

    public void SetHoleNumber(int holeNumber)
    {
        currentHole = holeNumber;
        UpdateHoleInfo();
    }

    void UpdateHoleInfo()
    {
        if (holeText != null)
        {
            holeText.text = "Hole " + currentHole;
        }
        else
        {
            Debug.LogError("HoleText is NULL! Make sure it's assigned in the Inspector.");
        }
    }
}
