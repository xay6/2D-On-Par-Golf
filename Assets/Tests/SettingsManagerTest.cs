using NUnit.Framework;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsManagerTest
{
    [Test]
    public void SetMasterVolume_ClampsAndStoresValue()
    {
        // Arrange
        var settingsGO = new GameObject("SettingsManager");
        var settings = settingsGO.AddComponent<SettingsManager>();

        var mixer = new AudioMixer(); // mock mixer
        typeof(SettingsManager)
            .GetField("audioMixer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(settings, mixer);

        // Act
        settings.SetMasterVolume(2f);  // out of range, should clamp to 1

        // Assert
        Assert.AreEqual(1f, settings.masterVolume);
    }
}
