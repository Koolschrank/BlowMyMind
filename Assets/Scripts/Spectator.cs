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
    [SerializeField] private Material[] faceMaterials;
    [SerializeField] private MeshRenderer playerRenderer;
    
    private Vector3 _startPosition;
    private Sequence _jumpSequence;

    private void Awake()
    {
        _startPosition = transform.position;
        var playerMaterials = playerRenderer.materials;
        playerMaterials[1] = faceMaterials[Random.Range(0, faceMaterials.Length)];
        playerRenderer.materials = playerMaterials;
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
