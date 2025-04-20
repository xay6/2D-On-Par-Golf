using UnityEngine;

public class BallSkinApplier : MonoBehaviour
{
    void Start()
    {
        // Apply the currently selected skin to this ball
        if (BallSkinManager.Instance != null)
        {
            BallSkinManager.Instance.ApplySkin(this.gameObject);
        }
        else
        {
            Debug.LogWarning("BallSkinManager is missing in the scene!");
        }
    }
}
