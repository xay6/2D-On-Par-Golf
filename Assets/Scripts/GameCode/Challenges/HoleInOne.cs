using UnityEngine;

public class HoleInOne : MonoBehaviour
{

public ChallengeUIManger uiManager;

    public void CheckHoleInOne(){
        if (uiManager == null){
            Debug.LogError("ChallengeUIManager is not assigned in HoleInOne script");
            return;
        }
        if (ScoreManager.Instance == null) {
            Debug.LogError("ScoreManager.Instance is null! Make sure it's in the scene.");
            return;
        }

        int s = ScoreManager.Instance.strokes;

        if (s == 1) {
            uiManager.ShowSuccessPanel();
            // Debug.Log("Success Panel Shown");

            if (CoinManager.Instance != null) {
                CoinManager.Instance.AddCoins(100);
            } else {
                Debug.LogError("CoinManager.Instance is null!");
            }
        } 
        else 
        {
            uiManager.ShowFailedPanel();
            Debug.Log("Failed Panel Shown");
        }
    }
}
