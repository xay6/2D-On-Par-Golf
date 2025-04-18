using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreTextInitializer : MonoBehaviour
{
    public bool isPostUI = false;
    public bool isChallengeUI = false;

    void Start()
    {
        if (ScoreManager.Instance != null)
        {
            var text = GetComponent<TextMeshProUGUI>();
            string sceneName = SceneManager.GetActiveScene().name;

            if (isPostUI)
            {
                ScoreManager.Instance.RegisterScoreTextPostUI(text);
            }
            else if (isChallengeUI && sceneName.StartsWith("Challenge")) // ✅ Only in challenge scenes
            {
                ScoreManager.Instance.RegisterChallengeScoreTextPostUI(text);
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
