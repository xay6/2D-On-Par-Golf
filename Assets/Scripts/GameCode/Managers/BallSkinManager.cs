using UnityEngine;

public class BallSkinManager : MonoBehaviour
{
    public static BallSkinManager Instance;

    public Material[] ballSkins; // Drag BallSkin_Red and others here
    public bool[] unlockedSkins; // Mark unlocked status per skin
    public int currentSkinIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            LoadSkinSelection();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ApplySkin(GameObject ball)
    {
        Renderer renderer = ball.GetComponent<Renderer>();
        if (renderer != null && currentSkinIndex < ballSkins.Length)
        {
            renderer.material = ballSkins[currentSkinIndex];
        }
    }

    public void SetSkin(int index)
    {
        if (IsSkinUnlocked(index))
        {
            currentSkinIndex = index;
            PlayerPrefs.SetInt("SelectedSkin", index);
            PlayerPrefs.Save();
        }
    }

    public void UnlockSkin(int index)
    {
        if (index < unlockedSkins.Length)
        {
            unlockedSkins[index] = true;
            PlayerPrefs.SetInt("SkinUnlocked_" + index, 1);
        }
    }

    public bool IsSkinUnlocked(int index)
    {
        return index >= 0 && index < unlockedSkins.Length && unlockedSkins[index];
    }

    private void LoadSkinSelection()
    {
        currentSkinIndex = PlayerPrefs.GetInt("SelectedSkin", 0);
        for (int i = 0; i < unlockedSkins.Length; i++)
        {
            unlockedSkins[i] = PlayerPrefs.GetInt("SkinUnlocked_" + i, i == 0 ? 1 : 0) == 1;
        }
    }
}
