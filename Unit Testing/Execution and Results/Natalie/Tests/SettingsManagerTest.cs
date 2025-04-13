using NUnit.Framework;
using UnityEngine;

public class SettingsManagerTest
{
    private SettingsManager settingsManager;

    [SetUp]
    public void Setup()
    {
        GameObject go = new GameObject("SettingsManager");
        settingsManager = go.AddComponent<SettingsManager>();
        settingsManager.Awake(); // manually invoke since Unity doesn't in tests
    }

    [Test]
    public void TestSetMasterVolume_ClampsAndStoresValue()
    {
        settingsManager.SetMasterVolume(2f);
        Assert.AreEqual(1f, settingsManager.masterVolume);
    }

    [Test]
    public void TestSetMusicVolume_ClampsAndStoresValue()
    {
        settingsManager.SetMusicVolume(-1f);
        Assert.AreEqual(0f, settingsManager.musicVolume);
    }

    [Test]
    public void TestSetSFXVolume_StoresAndPersists()
    {
        float expected = 0.77f;
        settingsManager.SetSFXVolume(expected);
        Assert.AreEqual(expected, settingsManager.soundfxVolume);
        Assert.AreEqual(expected, PlayerPrefs.GetFloat("SoundFXVolume"));
    }
}

