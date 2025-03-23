using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance; 
    private int coins = 0; 

    public TextMeshProUGUI coinTotalText; 
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 

            // PlayerPrefs.DeleteKey("GameStartedBefore"); Only if want to reset coin everytime

            if (IsFirstGameLaunch())
            {
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

    private void Start()
    {
        UpdateCoinUI();
    }

    public void AddCoins(int amount)
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
        if (coinTotalText != null)
        {
            coinTotalText.text = "Coins: " + coins;
        }
        else
        {
            Debug.LogError("CoinTotal Text UI is not assigned in CoinManager.");
        }
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
}