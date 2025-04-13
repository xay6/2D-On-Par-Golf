using UnityEngine;

public class ManualGameManagerTest : MonoBehaviour
{
    void Start()
    {
        RunScoreManagerTest();
        //RunSoundFXManagerTest();
        RunSettingsManagerTest();
    }

    void RunScoreManagerTest()
    {
        ScoreManager.Instance.strokes = 0;
        ScoreManager.Instance.AddStroke();

        if (ScoreManager.Instance.strokes == 1)
            Debug.Log("ScoreManager Test Passed!");
        else
            Debug.Log("ScoreManager Test Failed.");
    }

    void RunSoundFXManagerTest()
    {
        if (SoundFXManager.Instance != null)
        {
            SoundFXManager.Instance.PlaySoundEffect(null, null, 1f); // Replace with a real clip/transform if available
            Debug.Log("SoundFXManager Test Ran (check console for logs or audio).");
        }
        else
        {
            Debug.Log("SoundFXManager instance not found.");
        }
    }

    void RunSettingsManagerTest()
    {
        SettingsManager.Instance.SetVolume(0.5f);

        if (Mathf.Approximately(SettingsManager.Instance.masterVolume, 0.5f))
            Debug.Log("SettingsManager Volume Set Correctly.");
        else
            Debug.Log("SettingsManager Volume Test Failed.");
    }
}