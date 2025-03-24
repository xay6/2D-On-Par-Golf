using UnityEngine;

public class HoleInOne : MonoBehaviour
{

public ChallengeUIManger uiManager;

    public void CheckHoleInOne(){
        if (uiManager == null){
            Debug.LogError("ChallengeUIManager is not assigned in HoleInOne script");
            return;
        }

        int s = ScoreManager.Instance.strokes;

        if(s == 1 ){
            uiManager.ShowSuccessPanel();
            Debug.Log("Succucess Panel Show");

            CoinManager.Instance.AddCoins(100);
        }
        else{
            uiManager.ShowFailedPanel();
            Debug.Log("Failed Panel Show: ");
        }
    }
}
