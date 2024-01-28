using System;
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

    private Vector3 _cameraStartPosition;
    private float _cameraMaxHeight;
    private Tween _cameraRiseTween;
    private Tween _cameraLowerTween;

    private void Awake()
    {
        _cameraStartPosition = virtualCamera.position;
    }

    [Button("TestJumping")]
    public void TestJumpTrigger()
    {
        if(_cameraRiseTween != null && _cameraRiseTween.active)
            return;
        
        _cameraLowerTween?.Kill();
        
        StartAudienceJump?.Invoke();
        var startHeight = virtualCamera.position.y - _cameraStartPosition.y;
        _cameraRiseTween = DOVirtual.Float(startHeight, cameraRiseHeight, cameraRiseTime, height =>
        {
            virtualCamera.position = _cameraStartPosition + Vector3.up * height;
        }).SetEase(Ease.InOutQuad);
    }

    [Button("TestJumpStop")]
    public void TestJumpStop()
    {
        
        if(_cameraLowerTween != null && _cameraLowerTween.active)
            return;
        
        var startHeight = virtualCamera.position.y - _cameraStartPosition.y;
        _cameraLowerTween = DOVirtual.Float(startHeight, 0, cameraRiseTime, height =>
        {
            virtualCamera.position = _cameraStartPosition + Vector3.up * height;
        }).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            StopAudienceJump?.Invoke();
        });
    }
}
