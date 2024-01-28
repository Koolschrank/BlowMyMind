using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spectator : MonoBehaviour
{
    [SerializeField] private float timeForOneJump;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float maxRandomJumpDelay;
    [SerializeField] private ParticleSystem laughVFX;
    
    private Vector3 _startPosition;
    private Sequence _jumpSequence;

    private void Awake()
    {
        _startPosition = transform.position;
    }

    private void OnEnable()
    {
        AudienceManager.StartAudienceJump += StartJumping;
        AudienceManager.StopAudienceJump += StopJumping;
    }

    private void OnDisable()
    {
        AudienceManager.StartAudienceJump -= StartJumping;
        AudienceManager.StopAudienceJump -= StopJumping;
    }

    [Button("StartJumping")]
    public void StartJumping()
    {
        Invoke(nameof(Jump), Random.Range(0, maxRandomJumpDelay));    
    }

    
    private void Jump()
    {
        laughVFX.Play();
        _jumpSequence = DOTween.Sequence();
        _jumpSequence.Append(transform.DOMoveY(_startPosition.y + jumpHeight, timeForOneJump * 0.5f)
            .SetEase(Ease.OutSine));
        _jumpSequence.Append(transform.DOMoveY(_startPosition.y, timeForOneJump * 0.5f).SetEase(Ease.InSine));
        _jumpSequence.SetLoops(-1);
    }

    [Button("StopJumping")]
    public void StopJumping()
    {
        laughVFX.Stop();
        _jumpSequence?.Kill();
        transform.position = _startPosition;
    }
}
