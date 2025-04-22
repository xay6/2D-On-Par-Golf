using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance; 
    private static int coins = 0;
    public bool hasReward;

    public TextMeshProUGUI coinTotalText;
    public TextMeshProUGUI postCoinTotalText; 
    public TextMeshProUGUI challengeCoinText;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 

            // PlayerPrefs.DeleteKey("GameStartedBefore"); Only if want to reset coin everytime
            SceneManager.sceneLoaded += OnSceneLoaded;
            if (IsFirstGameLaunch())
            {
                hasReward = false;
                ResetCoins();
            }
            else
            {
                LoadCoins();
            }
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadCoins();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // No need to find UI here anymore — UI elements will register themselves!
        UpdateCoinUI();
    }

    private void Start()
    {
        UpdateCoinUI();
    }

    public void AddCoins(int amount)
    {
        if(TryClaimReward()){
        coins += amount;
        SaveCoins();
        UpdateCoinUI();
        }
    }

    public void AddCoinsDaily(int amount)
    {
        coins += amount;
        SaveCoins();
        UpdateCoinUI();
    }


    public int GetCoins()
    {
        return coins;
    }

    public void ResetCoins()
    {
        coins = 0;
        SaveCoins();
        UpdateCoinUI();
    }

    private void SaveCoins()
    {
        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.Save();
    }

    private void LoadCoins()
    {
        coins = PlayerPrefs.GetInt("Coins", 0);
    }

    private void UpdateCoinUI()
    {
        string coinsString  = "Coins: " + coins;

        if (coinTotalText != null)
        {
            coinTotalText.text = coinsString;
        }

        if (postCoinTotalText != null)
        {
            postCoinTotalText.text = coinsString;
        }

        if (challengeCoinText != null)
        {
            challengeCoinText.text = coinsString;
        }
    }

    public void RegisterCoinText(TextMeshProUGUI text)
    {
        coinTotalText = text;
        UpdateCoinUI();
    }

    public void RegisterPostCoinText(TextMeshProUGUI text)
    {
        postCoinTotalText = text;
        UpdateCoinUI();
    }

    public void ChallengeCoinText(TextMeshProUGUI text)
    {
        challengeCoinText = text;
        UpdateCoinUI();
    }

    

    public bool CheckReward(){
        if (hasReward){
            Debug.Log("Hole-in-One already rewarded.");
            return false;
        }else{
            hasReward = true;
            return true;
        }
    }

    public bool CanClaimReward()
    {
        return !hasReward;
    }

    public bool TryClaimReward()
    {
        if (!CanClaimReward())
        {
            Debug.Log("Hole-in-One already rewarded.");
            return false;
        }

        hasReward = true;
        return true;
    }


    private bool IsFirstGameLaunch()
    {
        if (!PlayerPrefs.HasKey("GameStartedBefore"))
        {
            PlayerPrefs.SetInt("GameStartedBefore", 1);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }
    public bool SpendCoins(int amount)
{
    if (coins >= amount)
    {
        coins -= amount;
        SaveCoins();
        UpdateCoinUI();
        return true;
    }
    else
    {
        Debug.Log(" Not enough coins.");
        return false;
    }
}
}