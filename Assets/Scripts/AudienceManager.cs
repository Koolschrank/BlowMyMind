using System;
using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class AudienceManager : MonoBehaviour
{
    public static Action StartAudienceJump;
    public static Action StopAudienceJump;

    [SerializeField] private Transform virtualCamera;
    [SerializeField] private float cameraRiseHeight;
    [SerializeField] private float cameraRiseTime;
    [SerializeField] private float cameraPauseTime;
    [SerializeField] private float cameraLowerTime;
    [SerializeField] private AudioSource audienceLaugh;

    private Vector3 _cameraStartPosition;
    private float _cameraMaxHeight;
    private Tween _cameraRiseTween;
    private Tween _cameraLowerTween;
    private float _currentPauseTimer;

    private void Awake()
    {
        _cameraStartPosition = virtualCamera.position;
    }

    [Button("TestJumping")]
    public void TestJumpTrigger()
    {
        audienceLaugh.Play();
        _currentPauseTimer = cameraPauseTime;
        if (_cameraRiseTween != null && _cameraRiseTween.active)
            return;
        
        _cameraLowerTween?.Kill();
        
        StartAudienceJump?.Invoke();
        var startHeight = virtualCamera.position.y - _cameraStartPosition.y;
        _cameraRiseTween = DOVirtual.Float(startHeight, cameraRiseHeight, cameraRiseTime, height =>
        {
            virtualCamera.position = _cameraStartPosition + Vector3.up * height;
        }).SetEase(Ease.InOutQuad).OnComplete(() => { StartCoroutine(StopAfterPause()); }) ;
    }

    private IEnumerator StopAfterPause()
    {
        do
        {
            _currentPauseTimer -= Time.deltaTime;
            yield return null;
        } while (_currentPauseTimer > 0);
        TestJumpStop();
    }

    [Button("TestJumpStop")]
    public void TestJumpStop()
    {
        
        if(_cameraLowerTween != null && _cameraLowerTween.active)
            return;
        
        var startHeight = virtualCamera.position.y - _cameraStartPosition.y;
        _cameraLowerTween = DOVirtual.Float(startHeight, 0, cameraLowerTime, height =>
        {
            virtualCamera.position = _cameraStartPosition + Vector3.up * height;
        }).SetEase(Ease.InQuad).OnComplete(() =>
        {
            StopAudienceJump?.Invoke();
        });
    }
}
