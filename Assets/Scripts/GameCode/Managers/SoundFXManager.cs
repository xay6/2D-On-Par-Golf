using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;


[DefaultExecutionOrder(-10)]
public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;
    [SerializeField] private AudioSource SoundFXObject;
    

private void Awake()
{
    if (instance == null)
    {
        Debug.Log("SoundFXManager instance assigned!");
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    else
    {
        Debug.LogWarning("Duplicate SoundFXManager found! Destroying this instance.");
        Destroy(gameObject);
    }
}


public void PlaySoundEffect(AudioClip audioClip, Transform spawnTransform, float volume)
{
   Debug.Log("Playing sound: " + audioClip.name);
    if (SoundFXObject == null)
    {
        Debug.LogError("Sound prefab is missing! Assign it in the Inspector.");
        return;
    }
    if (audioClip == null)
    {
        Debug.LogError("Audio clip is missing!");
        return;
    }

    // Instantiate the AudioSource at the given transform's position
    AudioSource audioSource = Instantiate(SoundFXObject, spawnTransform.position, Quaternion.identity);
    audioSource.clip = audioClip;
    audioSource.volume = volume;
    audioSource.spatialBlend = 0f; // 3D sound (set to 0 for 2D)
    audioSource.Play();

    // Destroy after the clip finishes playing
    Destroy(audioSource.gameObject, audioClip.length);
}
}
