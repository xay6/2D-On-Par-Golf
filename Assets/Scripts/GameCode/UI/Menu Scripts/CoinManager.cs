using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using OnPar.RouterHandlers;
using OnPar.Routers;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance; 
    private int coins = 0;
    public bool hasReward = false; 
    UserItems serverCoins;

    public TextMeshProUGUI coinTotalText;
    public TextMeshProUGUI postCoinTotalText; 
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
                ResetCoins();
            }
            else
            {
                SaveCoins();
                LoadCoins();
                Debug.Log(serverCoins.coinAmount);
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
        if(CheckReward()){
        coins += amount;
        SaveCoins();
        UpdateCoinUI();
        }
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

    private async void SaveCoins()
    {
        coins += serverCoins.coinAmount;
        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.Save();
        Debug.Log(coins);
        await Handlers.UpdateCoinsHandler(LoginRegister.getUsername(), coins);
    }

    private async void LoadCoins()
    {
        serverCoins = await Handlers.GetCoinsHandler(LoginRegister.getUsername());
        coins = PlayerPrefs.GetInt("Coins", serverCoins.coinAmount);
        Debug.Log(serverCoins);
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

    public bool CheckReward(){
        if (hasReward){
            // Debug.Log("Hole-in-One already rewarded.");
            return false;
        }else{
            hasReward = true;
            return true;
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