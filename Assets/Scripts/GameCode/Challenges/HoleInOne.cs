using UnityEngine;

public class HoleInOne : MonoBehaviour
{
    public ChallengeUIManager uiManager; // Reference to ChallengeUIManager
    private int strokeCount = 0;  // Number of strokes taken


    public void IncreaseStrokeCount()
    {
        strokeCount++;
        Debug.Log("Strokes: " + strokeCount);   
    }

    public void CheckHoleInOne()
    {
        if (uiManager == null)
        {
            Debug.LogError("ChallengeUIManager is not assigned in HoleInOne script!");
            return;
        }

        if (strokeCount == 1)
        {
            uiManager.ShowSuccessPanel();
        }
        else
        {
            uiManager.ShowFailedPanel();
        }
    }
}
