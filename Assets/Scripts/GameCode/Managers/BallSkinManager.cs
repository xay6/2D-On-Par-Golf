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
    SpriteRenderer sr = ball.GetComponent<SpriteRenderer>();
    if (sr != null && currentSkinIndex < ballSkins.Length)
    {
        sr.material = ballSkins[currentSkinIndex];
        sr.color = Color.white; 
    }
    else
    {
        Debug.LogWarning(" Could not apply skin: SpriteRenderer missing or index out of range.");
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

    public bool TryUnlockSkin(int index, int cost)
{
    if (!IsSkinUnlocked(index))
    {
        if (CoinManager.Instance.SpendCoins(cost))
        {
            UnlockSkin(index);
            Debug.Log("Skin " + index + " unlocked with coins!");
            return true;
        }
        else
        {
            Debug.LogWarning("Not enough coins to unlock skin " + index);
        }
    }
    return false;
}
}
