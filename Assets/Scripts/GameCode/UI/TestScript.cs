using UnityEngine;
using UnityEngine.InputSystem; 

public class SkinCycler : MonoBehaviour
{
    public GameObject ball;

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
            Debug.Log(" Set Skin to Index: " + prev);
        }
    }
}