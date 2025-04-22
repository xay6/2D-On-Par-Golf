using UnityEngine;

public class BallSkinManager : MonoBehaviour
{
    public static BallSkinManager Instance;

    public Material[] ballSkins; 
    public Sprite[] ballSkinImages; 
    public bool[] unlockedSkins;
    public bool[] unlockedSkinImages;
    
    public int currentSkinIndex = 0;
    public int currentSkinImageIndex = 0;
    public enum SkinMode { Material, Sprite }
    public SkinMode currentSkinMode;

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
        var sr = ball?.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogWarning("No SpriteRenderer found.");
            return;
        }

        if (currentSkinMode == SkinMode.Material)
        {
            if (currentSkinIndex < ballSkins.Length && ballSkins[currentSkinIndex] != null)
            {
                sr.material = ballSkins[currentSkinIndex];
                sr.color = Color.white;
                Debug.Log("Applied MATERIAL skin.");
            }
        }
        else if (currentSkinMode == SkinMode.Sprite)
        {
            if (currentSkinImageIndex < ballSkinImages.Length && ballSkinImages[currentSkinImageIndex] != null)
            {
                sr.sprite = ballSkinImages[currentSkinImageIndex];
                //sr.material = sharedMaterial;
                
                sr.color = Color.white;
                Debug.Log("Applied SPRITE skin.");
            }
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
        int materialCount = ballSkins?.Length ?? 0;
        int spriteCount = ballSkinImages?.Length ?? 0;

        if (unlockedSkins == null || unlockedSkins.Length != materialCount)
            unlockedSkins = new bool[materialCount];

        if (unlockedSkinImages == null || unlockedSkinImages.Length != spriteCount)
            unlockedSkinImages = new bool[spriteCount];

        for (int i = 0; i < materialCount; i++)
            unlockedSkins[i] = PlayerPrefs.GetInt("SkinUnlocked_" + i, i == 0 ? 1 : 0) == 1;

        for (int i = 0; i < spriteCount; i++)
            unlockedSkinImages[i] = PlayerPrefs.GetInt("SkinUnlockedSprite_" + i, i == 0 ? 1 : 0) == 1;

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
