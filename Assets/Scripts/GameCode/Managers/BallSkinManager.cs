using UnityEngine;

public class BallSkinManager : MonoBehaviour
{
    public static BallSkinManager Instance;

    public Material[] ballSkins;
    public int unlockedSkinCount = 1; // default unlocked
    public int currentSkinIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ApplySkin(GameObject ball)
    {
        Renderer rend = ball.GetComponent<Renderer>();
        if (rend != null && currentSkinIndex < ballSkins.Length)
        {
            rend.material = ballSkins[currentSkinIndex];
        }
    }

    public void UnlockSkin(int index)
    {
        if (index > unlockedSkinCount)
        {
            unlockedSkinCount = index;
        }
    }

    public bool IsSkinUnlocked(int index)
    {
        return index < unlockedSkinCount;
    }

    public void SetSkin(int index)
    {
        if (IsSkinUnlocked(index))
        {
            currentSkinIndex = index;
        }
    }
}
