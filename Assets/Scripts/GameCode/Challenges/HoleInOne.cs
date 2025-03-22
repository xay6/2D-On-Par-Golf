using UnityEngine;

public class HoleInOne : MonoBehaviour
{

public ChallengeUIManger uiManager;
 private bool hasRewarded = false;

    public void CheckHoleInOne(){
        if (uiManager == null){
            Debug.LogError("ChallengeUIManager is not assigned in HoleInOne script");
            return;
        }

         if (hasRewarded) 
        {
            Debug.Log("Hole-in-One already rewarded.");
            return;
        }

        int s = ScoreManager.Instance.strokes;

        if(s == 1 ){
            uiManager.ShowSuccessPanel();
            Debug.Log("Succucess Panel Show");

            CoinManager.Instance.AddCoins(100);
            hasRewarded = true;
        }
        else{
            uiManager.ShowFailedPanel();
            Debug.Log("Failed Panel Show: ");
        }
    }
}
