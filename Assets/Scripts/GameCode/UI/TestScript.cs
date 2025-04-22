using UnityEngine;
using UnityEngine.InputSystem;

public class TestScript : MonoBehaviour
{
    public GameObject ball;
    public SpriteRenderer previewRenderer;
    public int skinCost = 50;
    public int currentIndex = 0;

    private int totalSkins => BallSkinManager.Instance.ballSkins.Length + BallSkinManager.Instance.ballSkinImages.Length;

    void Start()
    {
        CoinManager.Instance.AddCoins(9999);

        // Unlocks everything
        for (int i = 0; i < BallSkinManager.Instance.ballSkins.Length; i++)
            BallSkinManager.Instance.unlockedSkins[i] = true;

        for (int i = 0; i < BallSkinManager.Instance.ballSkinImages.Length; i++)
            BallSkinManager.Instance.unlockedSkinImages[i] = true;

        ApplyCurrentSkin();
    }

    void Update()
    {
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            CycleSkin(1);
        }

        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            CycleSkin(-1);
        }
    }

    void CycleSkin(int direction)
    {
        int max = totalSkins;

        do
        {
            currentIndex = (currentIndex + direction + max) % max;
        }
        while (!IsSkinUnlocked(currentIndex));

        ApplyCurrentSkin();
    }

    bool IsSkinUnlocked(int index)
    {
        if (index < BallSkinManager.Instance.ballSkins.Length)
            return BallSkinManager.Instance.unlockedSkins[index];
        else
        {
            int spriteIndex = index - BallSkinManager.Instance.ballSkins.Length;
            return BallSkinManager.Instance.unlockedSkinImages[spriteIndex];
        }
    }

    void ApplyCurrentSkin()
    {
        var manager = BallSkinManager.Instance;

        if (currentIndex < manager.ballSkins.Length)
        {
            // It's a material skin
            manager.currentSkinIndex = currentIndex;
            manager.currentSkinMode = BallSkinManager.SkinMode.Material;
        }
        else
        {
            // It's a sprite skin
            manager.currentSkinImageIndex = currentIndex - manager.ballSkins.Length;
            manager.currentSkinMode = BallSkinManager.SkinMode.Sprite;
        }

        manager.ApplySkin(ball);

        // Show preview if sprite
        if (previewRenderer != null)
        {
            if (manager.currentSkinMode == BallSkinManager.SkinMode.Sprite)
            {
                previewRenderer.sprite = manager.ballSkinImages[manager.currentSkinImageIndex];
            }
            else
            {
                previewRenderer.sprite = null; // clear preview for materials
            }
        }

        Debug.Log($"Applied {(manager.currentSkinMode == BallSkinManager.SkinMode.Sprite ? "Sprite" : "Material")} Skin at index {currentIndex}");
    }
}
