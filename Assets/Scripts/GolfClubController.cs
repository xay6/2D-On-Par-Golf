using UnityEngine;
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) currentClubIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) currentClubIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) currentClubIndex = 2;

        if (clubText != null)
        {
            clubText.text = "current club: " + CurrentClub.clubName;
        }
    }
}