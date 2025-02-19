using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public class GolfClub
{
    public string clubName;
    public float forceMultiplier;
    public float ballMass;
    public float linearDamping;
    public float angularDamping;
}

public class GolfClubController : MonoBehaviour
{
    public List<GolfClub> clubs;
    public TextMeshProUGUI clubText;

    private int currentClubIndex = 0;

    public GolfClub CurrentClub => clubs[currentClubIndex];
    private LaunchWithDrag launchWithDrag;

    private void Start()
    {
        launchWithDrag = GameObject.Find("Ball").GetComponent<LaunchWithDrag>();
        UpdateClubText();
        UpdateClubStats();
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

    private void UpdateClubStats() {
        launchWithDrag.setForce(CurrentClub.forceMultiplier);
        launchWithDrag.AngularDrag = CurrentClub.angularDamping;
        launchWithDrag.LinearDrag = CurrentClub.linearDamping;
    }

    private void Update()
    {
        if(!launchWithDrag.isMoving()) {
            var keyboard = Keyboard.current;
            
            if (keyboard.digit1Key.wasPressedThisFrame)
            {
                currentClubIndex = 0;
                UpdateClubText();
                UpdateClubStats();
            }

            if (keyboard.digit2Key.wasPressedThisFrame)
            {
                currentClubIndex = 1;
                UpdateClubText();
                UpdateClubStats();
            }

            if (keyboard.digit3Key.wasPressedThisFrame)
            {
                currentClubIndex = 2;
                UpdateClubText();
                UpdateClubStats();
            }
        }
    }
}