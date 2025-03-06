using UnityEngine;

public class HoleInOne : MonoBehaviour
{

public ChallengeUIManger uiManager;

    public void CheckHoleInOne(){
        if (uiManager == null){
            Debug.LogError("ChallengeUIManager is not assigned in HoleInOne script");
            return;
        }

        ScoreManager.Instance.AddStroke();
        int s = ScoreManager.Instance.strokes;

        if(s == 1 ){
            uiManager.ShowSuccessPanel();
            Debug.Log("Succucess Panel Show");
        }
        else{
            uiManager.ShowFailedPanel();
            Debug.Log("Failed Panel Show");
        }
    }
}
