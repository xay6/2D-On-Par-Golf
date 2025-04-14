using UnityEngine;
using TMPro;

public class CoinTextInitializer : MonoBehaviour
{
    public bool isPostUI = false;

    void Start()
    {
        if (CoinManager.Instance != null)
        {
            var text = GetComponent<TextMeshProUGUI>();
            if (isPostUI)
            {
                CoinManager.Instance.RegisterPostCoinText(text);
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
