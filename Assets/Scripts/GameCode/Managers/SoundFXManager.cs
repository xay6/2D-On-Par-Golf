using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;
    [SerializeField] private AudioSource SoundFXObject;
    

    private void Awake()
    {
        if (instance==null)
        {
            instance = this;
        }
    }


    public void PlaySoundEffect(AudioClip audioClip, Transform spawnTransform, float volume)
{
    if (SoundFXObject == null)
    {
        Debug.LogError("Sound prefab is missing! Assign it in the Inspector.");
        return;
    }
    if (audioClip == null)
    {
        Debug.LogError("audio is missing");
    }

    // Instantiate the AudioSource at the given transform's position
    AudioSource audioSource = Instantiate(SoundFXObject, spawnTransform.position, Quaternion.identity);
    audioSource.clip = audioClip;
    audioSource.volume = volume;
    audioSource.spatialBlend = 1f; // 3D sound (set to 0 for 2D)
    audioSource.Play();

    // Destroy after the clip finishes playing
    Destroy(audioSource.gameObject, audioClip.length);
}
}
