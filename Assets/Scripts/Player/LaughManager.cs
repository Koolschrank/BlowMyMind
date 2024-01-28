using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Player;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class LaughManager : MonoBehaviour
{
    [SerializeField] private AudioSource laugh1;
    [SerializeField] private AudioSource laugh2;
    [SerializeField] private AudioSource laugh3;
    [SerializeField] private bool useLaugh = true;
    [SerializeField] private float threshold1;
    [SerializeField] private float threshold2;
    [SerializeField] private float threshold3;
    [SerializeField] private float maxVolume;
    [SerializeField] private float fadeTime;
    [SerializeField] private PlayerCharacter playerCharacter;

    private float _threshold1Up;
    private float _threshold1Down;
    private float _threshold2Up;
    private float _threshold2Down;
    private float _threshold3Up;
    private float _threshold3Down;
    
    private int _currentLaughLevel;
    

    private void Awake()
    {
        _threshold2Up = threshold2 + 5f;
        _threshold2Down = threshold2 - 5f;
        _threshold3Up = threshold3 + 5f;
        _threshold3Down = threshold3 - 5f;
        playerCharacter = GetComponent<PlayerCharacter>();
    }

    public void OnDamageChange()
    {
        if(!useLaugh)
            return;

        var newValue = playerCharacter.damage.Value;

        switch (_currentLaughLevel)
        {
            case 0:
                if (newValue > threshold1 + 2)
                {
                    _currentLaughLevel = 1;
                    UpdateLaugh();
                }
                break;
            case 1:
                if (newValue > _threshold2Up)
                {
                    _currentLaughLevel = 2;
                    UpdateLaugh();
                    break;
                }
                if (newValue < threshold1)
                {
                    _currentLaughLevel = 0;
                    UpdateLaugh();
                }
                break;
            case 2:
                if (newValue > _threshold3Up)
                {
                    _currentLaughLevel = 3;
                    UpdateLaugh();
                    break;
                }

                if (newValue < _threshold2Down)
                {
                    _currentLaughLevel = 1;
                    UpdateLaugh();
                }
                break;
            case 3:
                if (newValue < _threshold3Down)
                {
                    _currentLaughLevel = 2;
                    UpdateLaugh();
                }
                break;
        }
    }

    private void UpdateLaugh()
    {
        switch (_currentLaughLevel)
        {
            case 0:
                SilenceLaugh(laugh1);
                SilenceLaugh(laugh2);
                SilenceLaugh(laugh3);
                break;
            case 1:
                StartLaugh(laugh1);
                SilenceLaugh(laugh2);
                SilenceLaugh(laugh3);
                break;
            case 2:
                SilenceLaugh(laugh1);
                StartLaugh(laugh2);
                SilenceLaugh(laugh3);
                break;
            case 3:
                SilenceLaugh(laugh1);
                SilenceLaugh(laugh2);
                StartLaugh(laugh3);
                break;
        }
    }

    private void SilenceLaugh(AudioSource laugh)
    {
        var startVolume = laugh.volume;
        DOVirtual.Float(startVolume, 0, fadeTime, volume =>
        {
            laugh.volume = volume;
        }).SetEase(Ease.OutSine);
    }

    private void StartLaugh(AudioSource laugh)
    {
        var startVolume = laugh.volume;
        DOVirtual.Float(startVolume, maxVolume, fadeTime, volume =>
        {
            laugh.volume = volume;
        }).SetEase(Ease.OutSine);
    }
}
