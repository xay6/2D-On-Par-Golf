using NUnit.Framework;
using UnityEngine;

public class SoundFXManagerTest
{
   /* [Test]
    public void PlaySoundEffect_CreatesAudioSource()
    {
        // Arrange
        var soundManagerGO = new GameObject("SoundFXManager");
        var soundManager = soundManagerGO.AddComponent<SoundFXManager>();

        var target = new GameObject("Target");
        var testClip = AudioClip.Create("TestClip", 44100, 1, 44100, false);

        // Act
        soundManager.PlaySoundEffect(testClip, target.transform, 1f);
        var audioSource = target.GetComponent<AudioSource>();

        // Assert
        Assert.IsNotNull(audioSource);
        Assert.AreEqual(testClip, audioSource.clip);
        Assert.IsTrue(audioSource.isPlaying);
    }*/
}