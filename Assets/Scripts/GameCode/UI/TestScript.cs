using UnityEngine;
using UnityEngine.InputSystem;

public class SkinCycler : MonoBehaviour
{
    public GameObject ball;
    public int skinCost = 50;

    void Start()
{
    // Ensure unlockedSkins[] is the correct size
    if (BallSkinManager.Instance.unlockedSkins.Length != BallSkinManager.Instance.ballSkins.Length)
    {
        BallSkinManager.Instance.unlockedSkins = new bool[BallSkinManager.Instance.ballSkins.Length];
    }

    // Give coins and unlock all
    CoinManager.Instance.AddCoins(9999);
    for (int i = 0; i < BallSkinManager.Instance.ballSkins.Length; i++)
    {
        BallSkinManager.Instance.TryUnlockSkin(i, 50);
    }

    BallSkinManager.Instance.ApplySkin(ball);
}


    void Update()
    {
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            int next = (BallSkinManager.Instance.currentSkinIndex + 1) % BallSkinManager.Instance.ballSkins.Length;
            BallSkinManager.Instance.SetSkin(next);
            BallSkinManager.Instance.ApplySkin(ball);
            Debug.Log("Set Skin to Index: " + next);
        }

        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            int prev = (BallSkinManager.Instance.currentSkinIndex - 1 + BallSkinManager.Instance.ballSkins.Length) % BallSkinManager.Instance.ballSkins.Length;
            BallSkinManager.Instance.SetSkin(prev);
            BallSkinManager.Instance.ApplySkin(ball);
            Debug.Log("Set Skin to Index: " + prev);
        }
    }
}
