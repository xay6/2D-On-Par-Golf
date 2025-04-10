using UnityEngine;
using TMPro;

public class ScoreTextInitializer : MonoBehaviour
{
    public bool isPostUI = false;

    void Start()
    {
        if (ScoreManager.Instance != null)
        {
            var text = GetComponent<TextMeshProUGUI>();
            if (isPostUI)
            {
                ScoreManager.Instance.RegisterScoreTextPostUI(text);
            }
            else
            {
                ScoreManager.Instance.RegisterScoreText(text);
            }
        }
        else
        {
            Debug.LogWarning("ScoreManager instance not found in scene!");
        }
    }
}
