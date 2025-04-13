using NUnit.Framework;
using UnityEngine;

public class SoundEffectsTest
{
    private GameObject sfxManagerObj;
    private SoundFXManager sfxManager;

    [SetUp]
    public void Setup()
    {
        sfxManagerObj = new GameObject("SoundFXManager");
        sfxManager = sfxManagerObj.AddComponent<SoundFXManager>();
    }

    [Test]
    public void TestInstanceCreation()
    {
        Assert.IsNotNull(SoundFXManager.Instance);
    }

    [Test]
    public void TestPlaySoundEffect_AddsAudioSource()
    {
        var clip = AudioClip.Create("test", 44100, 1, 44100, false);
        sfxManager.PlaySoundEffect(clip, sfxManager.transform);
        Assert.IsTrue(sfxManager.transform.GetComponent<AudioSource>() != null);
    }
}
