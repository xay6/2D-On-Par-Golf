using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using TMPro;
using System.Collections;

public class CoinManagerTest
{
    private GameObject coinManagerObject;
    private CoinManager coinManager;
    private GameObject dummyTMPObject;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Step 1: Create dummy TMP UI **first**
        dummyTMPObject = new GameObject("DummyTMP");
        var tmpText = dummyTMPObject.AddComponent<TextMeshProUGUI>();

        // Step 2: Create CoinManager GameObject, but keep it inactive
        coinManagerObject = new GameObject("CoinManagerTestObject");
        coinManagerObject.SetActive(false); // Prevent Awake() from running immediately

        // Step 3: Add CoinManager component
        coinManager = coinManagerObject.AddComponent<CoinManager>();

        // Step 4: Assign dummy TMP to avoid null errors
        coinManager.coinTotalText = tmpText;

        // Step 5: Now safely activate to trigger Awake()
        coinManagerObject.SetActive(true);

        // Step 6: Clean PlayerPrefs
        PlayerPrefs.DeleteKey("Coins");
        PlayerPrefs.DeleteKey("GameStartedBefore");

        yield return null;
    }

    [UnityTest]
    public IEnumerator AddCoins_ShouldIncreaseCoinCount_WhenRewardIsAvailable()
    {
        // Assert initial state
        Assert.AreEqual(0, coinManager.GetCoins(), "Initial coins should be 0.");

        // Act
        coinManager.AddCoins(50);
        yield return null;

        Assert.AreEqual(50, coinManager.GetCoins(), "Coins should be 50 after adding 50.");

        // Reset reward status to simulate next level
        coinManager.hasReward = false;

        // Act again
        coinManager.AddCoins(30);
        yield return null;

        Assert.AreEqual(80, coinManager.GetCoins(), "Coins should be 80 after adding 30 more coins.");
    }

    [UnityTest]
    public IEnumerator AddCoins_ShouldNotIncreaseCoins_WhenRewardAlreadyClaimed()
    {
        // Arrange
        coinManager.AddCoins(50);
        yield return null;

        int coinsAfterFirstReward = coinManager.GetCoins();

        // Act — Try adding coins again without resetting reward
        coinManager.AddCoins(50);
        yield return null;

        // Assert
        Assert.AreEqual(coinsAfterFirstReward, coinManager.GetCoins(), "Coins should not increase after reward is already claimed.");
    }

    [UnityTest]
    public IEnumerator ResetCoins_ShouldSetCoinsToZero()
    {
        // Arrange
        coinManager.AddCoins(100);
        yield return null;

        Assert.AreEqual(100, coinManager.GetCoins(), "Coins should be 100 before reset.");

        // Act
        coinManager.ResetCoins();
        yield return null;

        Assert.AreEqual(0, coinManager.GetCoins(), "Coins should be 0 after reset.");
    }
    [UnityTearDown]
    public IEnumerator TearDown()
    {
        if (coinManagerObject != null) Object.Destroy(coinManagerObject);
        if (dummyTMPObject != null) Object.Destroy(dummyTMPObject);

        PlayerPrefs.DeleteKey("Coins");
        PlayerPrefs.DeleteKey("GameStartedBefore");

        yield return null;
    }
}
