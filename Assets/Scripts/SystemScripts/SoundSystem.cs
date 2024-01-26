using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SoundSystem : MonoBehaviour
{
    
    
    public static SoundSystem instance;
    [SerializeField] AudioSource audioSource;

    private void Start()
    {
        instance = this;
    
    }

    public void PlaySoundEffect(SoundEffectValue soundEffectValue)
    {
        audioSource.PlayOneShot(soundEffectValue.Clip, soundEffectValue.Volume);
    }
}


[Serializable]
public class SoundEffectValue
{
    
    [SerializeField] AudioClip clip;
    [Range(0,1)]
    [SerializeField] float volume;

    // getters

   
    public AudioClip Clip
    {
        get { return clip; }
    }

    public float Volume
    {
        get { return volume; }
    }

    public SoundEffectValue(AudioClip clip, float volume)
    {
        this.clip = clip;
        this.volume = volume;
    }

    public void Play()
    {
        if(volume ==0 || clip == null)
            return;
        SoundSystem.instance?.PlaySoundEffect(this);
    }
}