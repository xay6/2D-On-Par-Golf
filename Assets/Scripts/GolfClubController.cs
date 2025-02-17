using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public class GolfClub
{
    public string clubName;
    public float maxForce;
    public float heightFactor;
}

public class GolfClubController : MonoBehaviour
{
    public List<GolfClub> clubs;
    public TextMeshProUGUI clubText;

    private int currentClubIndex = 0;

    public GolfClub CurrentClub => clubs[currentClubIndex];

    private void Start()
    {
        UpdateClubText();
    }

    private void UpdateClubText()
    {
        if (clubText != null)
        {
            clubText.text = "Current Club: " + CurrentClub.clubName;
        }
        else
        {
            Debug.LogError("ClubText is not assigned in the Inspector!");
        }
    }

    private void Update()
    {
        var keyboard = Keyboard.current;

        if (keyboard.digit1Key.wasPressedThisFrame)
        {
            currentClubIndex = 0;
            UpdateClubText();
        }

        if (keyboard.digit2Key.wasPressedThisFrame)
        {
            currentClubIndex = 1;
            UpdateClubText();
        }

        if (keyboard.digit3Key.wasPressedThisFrame)
        {
            currentClubIndex = 2;
            UpdateClubText();
        }
    }
}