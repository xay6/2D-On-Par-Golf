using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CoinTextInitializer : MonoBehaviour
{
    public bool isPostUI = false;
    public bool isChallengeUI = false;
    
    void Start()
    {
        if (CoinManager.Instance != null)
        {
            var text = GetComponent<TextMeshProUGUI>();
            string sceneName = SceneManager.GetActiveScene().name;
            if (isPostUI)
            {
                CoinManager.Instance.RegisterPostCoinText(text);
            }
            else if (isChallengeUI && sceneName.StartsWith("Challenge"))
            {
                CoinManager.Instance.ChallengeCoinText(text);
            }
            else
            {
                CoinManager.Instance.RegisterCoinText(text);
            }
        }
        else
        {
            Debug.LogWarning("CoinManager instance not found in scene!");
        }
    }
}
