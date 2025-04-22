using UnityEngine;

public class BallSkinApplier : MonoBehaviour
{
    void Start()
    {
        if (BallSkinManager.Instance != null)
        {
            BallSkinManager.Instance.ApplySkin(this.gameObject);
            Debug.Log("Skin reapplied on ball at scene start.");
        }
        else
        {
            Debug.LogWarning("BallSkinManager not found when trying to apply skin.");
        }
    }
}