using UnityEngine;

public class BallSkinManager : MonoBehaviour {
    public static BallSkinManager Instance;

    public Sprite[] ballSkinImages;         
    public bool[] unlockedSkinImages;       
    public int currentSkinImageIndex = 0;   
    public Sprite defaultBallSprite;        
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
            Debug.LogWarning("No SpriteRenderer found on ball.");
            return;
        }

        if (currentSkinImageIndex < ballSkinImages.Length && ballSkinImages[currentSkinImageIndex] != null)
        {
            sr.sprite = ballSkinImages[currentSkinImageIndex];
            //sr.material = defaultBallSprite;
            sr.color = Color.white;
            Debug.Log("Applied sprite skin: " + ballSkinImages[currentSkinImageIndex].name);
        }
        else
        {
            sr.sprite = defaultBallSprite;
            //sr.material = null;
            sr.color = Color.white;
            Debug.LogWarning("Invalid or missing skin. Default applied.");
        }
    }

    public void SetSkin(int index)
    {
        if (IsSkinUnlocked(index))
        {
            currentSkinImageIndex = index;
            PlayerPrefs.SetInt("SelectedSpriteSkin", index);
            PlayerPrefs.Save();
        }
    }

    public void UnlockSkin(int index)
    {
        if (index < unlockedSkinImages.Length)
        {
            unlockedSkinImages[index] = true;
            PlayerPrefs.SetInt("SkinUnlockedSprite_" + index, 1);
        }
    }

    public bool IsSkinUnlocked(int index)
    {
        return index >= 0 && index < unlockedSkinImages.Length && unlockedSkinImages[index];
    }

    public bool TryUnlockSkin(int index, int cost)
    {
        if (!IsSkinUnlocked(index))
        {
            if (CoinManager.Instance.SpendCoins(cost))
            {
                UnlockSkin(index);
                Debug.Log("Skin " + index + " unlocked!");
                return true;
            }
            else
            {
                Debug.LogWarning("Not enough coins to unlock skin " + index);
            }
        }
        return false;
    }

    private void LoadSkinSelection()
    {
        int spriteCount = ballSkinImages?.Length ?? 0;

        if (unlockedSkinImages == null || unlockedSkinImages.Length != spriteCount)
            unlockedSkinImages = new bool[spriteCount];

        // Set default if no skin saved
        if (!PlayerPrefs.HasKey("SelectedSpriteSkin"))
        {
            currentSkinImageIndex = 0;
            PlayerPrefs.SetInt("SelectedSpriteSkin", currentSkinImageIndex);
            PlayerPrefs.Save();
        }
        else
        {
            currentSkinImageIndex = PlayerPrefs.GetInt("SelectedSpriteSkin", 0);
        }

        // Unlock the first skin by default
        for (int i = 0; i < spriteCount; i++)
        {
            unlockedSkinImages[i] = PlayerPrefs.GetInt("SkinUnlockedSprite_" + i, i == 0 ? 1 : 0) == 1;
        }
    }
}
