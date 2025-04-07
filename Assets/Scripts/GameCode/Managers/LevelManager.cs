using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    [SerializeField] private GameObject levelCompleteUI;
    [HideInInspector] public bool levelCompleted;

     void Awake()
    {
        main = this;
    }
    public void LevelComplete(){
        levelCompleted = true;
        levelCompleteUI.SetActive(true);
    }
}
