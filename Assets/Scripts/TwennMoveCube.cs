using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TwennMoveCube : MonoBehaviour
{
    Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        transform.DOMove(startPos +new Vector3(5, 0, 0), 1.0f).SetEase(Ease.InOutCirc).SetLoops(-1, LoopType.Yoyo); 
        transform.DORotate(new Vector3(0, 360, 0), 1.0f, RotateMode.FastBeyond360).SetEase(Ease.InOutCirc).SetLoops(-1, LoopType.Restart);
        //transform.DOMove(startPos + new Vector3(0, 0, 1), 0.5f).SetEase(Ease.InOutCirc).SetLoops(-1, LoopType.Yoyo);
        return;

        var sequence = DOTween.Sequence();

        sequence.Append(transform.DOMove(startPos + new Vector3(0, 0, 1), 0.5f).SetEase(Ease.InOutCirc));
        sequence.Append(transform.DOMove(startPos + new Vector3(0, 0, 2), 0.5f).SetEase(Ease.InOutCirc));
        sequence.Append(transform.DOMove(startPos + new Vector3(0, 0, 3), 0.5f).SetEase(Ease.InOutCirc));

        // this shows ho to use OnComplete
        //transform.DOMove(startPos +new Vector3(0, 0, 5), 1.0f).SetEase(Ease.InOutCirc).OnComplete(() =>
        //{
        //    transform.DORotate(new Vector3(0, 360, 0), 1.0f, RotateMode.FastBeyond360).SetEase(Ease.InOutCirc).SetLoops(-1, LoopType.Restart);
        //});
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
