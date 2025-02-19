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
    public TextMeshProUGUI clubText;

    public ClubStats[] clubs;
    private ClubStats CurrentClub;
    private int currentClubIndex = 0;

    private LaunchWithDrag launchWithDrag;

    private void Start()
    {
        if(clubs.Length > 0) {
            launchWithDrag = GameObject.Find("Ball").GetComponent<LaunchWithDrag>();
            CurrentClub = clubs[currentClubIndex];
            UpdateClubText();
            UpdateClubStats();
        }
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
                CurrentClub = clubs[currentClubIndex];
                UpdateClubText();
                UpdateClubStats();
            }

            if (keyboard.digit2Key.wasPressedThisFrame)
            {
                currentClubIndex = 1;
                CurrentClub = clubs[currentClubIndex];
                UpdateClubText();
                UpdateClubStats();
            }

            if (keyboard.digit3Key.wasPressedThisFrame)
            {
                currentClubIndex = 2;
                CurrentClub = clubs[currentClubIndex];
                UpdateClubText();
                UpdateClubStats();
            }
        }
    }
}