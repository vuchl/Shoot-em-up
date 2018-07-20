using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{

    private AudioSource audioSource;
    

    public void PlayAudioClip(AudioClip clip)
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(clip);
    }

    public void PlayAudioClipWithRandomPitch(AudioClip clip)
    {
        float pitchMin = 0.75f;
        float pitchMax = 1.25f;
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = Random.Range(pitchMin, pitchMax);
        audioSource.PlayOneShot(clip);
    }
}
