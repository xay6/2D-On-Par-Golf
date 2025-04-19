using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{
    public void LoadLevel(string LevelName){
        SceneManager.LoadSceneAsync(LevelName, LoadSceneMode.Single);
        ScoreManager.Instance.ResetStrokes();
    }

    public void ReloadLevel(){
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
