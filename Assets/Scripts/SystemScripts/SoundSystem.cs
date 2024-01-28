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

    public void SetPitch()
    {
        audioSource.pitch = 1;
    }

    public void SetPitch(float pitch)
    {
        audioSource.pitch = pitch;
    }

    public void PlaySoundEffect(SoundEffectValue soundEffectValue)
    {
        audioSource.PlayOneShot(soundEffectValue.Clip, soundEffectValue.Volume);
    }
}

[Serializable] 
public class SoundEffectValue_Array
{
    [SerializeField] SoundEffectValue[] soundEffectValues;

    public void Play()
    {
        // play random sound effect
        soundEffectValues[UnityEngine.Random.Range(0, soundEffectValues.Length-1)].Play();
        
    }
}



    [Serializable]
public class SoundEffectValue
{
    
    [SerializeField] AudioClip clip;
    [Range(0,1)]
    [SerializeField] float volume;
    [SerializeField] bool hasPitchVariation;
    [SerializeField] float pitchVariation;

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
        if (hasPitchVariation)
        {
               SoundSystem.instance?.SetPitch(UnityEngine.Random.Range(1 - pitchVariation, 1 + pitchVariation));
        }
        else
        {
               SoundSystem.instance?.SetPitch();
        }

            if (volume ==0 || clip == null)
            return;
        SoundSystem.instance?.PlaySoundEffect(this);
    }
}