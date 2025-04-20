using UnityEngine;

public class SkinTest : MonoBehaviour
{
    public GameObject ball;

    void Start()
    {
        CoinManager.Instance.AddCoins(150);

        bool unlocked = BallSkinManager.Instance.TryUnlockSkin(1, 100);

        if (unlocked)
        {
            BallSkinManager.Instance.SetSkin(1);
            BallSkinManager.Instance.ApplySkin(ball);
        }
    }
}








