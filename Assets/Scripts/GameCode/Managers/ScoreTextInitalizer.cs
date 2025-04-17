using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

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
                ScoreManager.Instance.RegisterChallengeScoreTextPostUI(text);
            }
        }
        else
        {
            Debug.LogWarning("ScoreManager instance not found in scene!");
        }
    }
}
