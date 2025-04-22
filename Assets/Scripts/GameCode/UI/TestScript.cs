using UnityEngine;
using UnityEngine.InputSystem;

public class TestScript : MonoBehaviour
{
    public GameObject ball;
    public SpriteRenderer previewRenderer;
    public int skinCost = 50;
    public int currentIndex = 0;

    void Start()
    {
        CoinManager.Instance.AddCoins(9999);

        // Unlock all sprite skins
        var manager = BallSkinManager.Instance;
        int spriteCount = manager.ballSkinImages.Length;

        if (manager.unlockedSkinImages == null || manager.unlockedSkinImages.Length != spriteCount)
            manager.unlockedSkinImages = new bool[spriteCount];

        for (int i = 0; i < spriteCount; i++)
            manager.unlockedSkinImages[i] = true;

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
        var manager = BallSkinManager.Instance;
        int max = manager.ballSkinImages.Length;

        do
        {
            currentIndex = (currentIndex + direction + max) % max;
        }
        while (!manager.unlockedSkinImages[currentIndex]);

        ApplyCurrentSkin();
    }

    void ApplyCurrentSkin()
    {
        var manager = BallSkinManager.Instance;

        manager.currentSkinImageIndex = currentIndex;
        PlayerPrefs.SetInt("SelectedSpriteSkin", currentIndex);
        PlayerPrefs.Save();

        manager.ApplySkin(ball);

        // Preview the sprite skin
        if (previewRenderer != null)
        {
            previewRenderer.sprite = manager.ballSkinImages[currentIndex];
        }

        Debug.Log($"Applied Sprite Skin: {currentIndex}");
    }
}
