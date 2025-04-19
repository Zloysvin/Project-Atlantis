using DG.Tweening;
using UnityEngine;

public class FeedbackScaler : MonoBehaviour
{
    [Header("General settngs:")]
    [SerializeField] float duration = 1.0f;
    [SerializeField] Ease ease = Ease.Linear;
    [SerializeField] int loops = 1;
    [SerializeField] LoopType loopType = LoopType.Yoyo;
    [SerializeField] Vector3 endPosition;
    [SerializeField,Tooltip("Percentage values.")] Vector3 fromPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 endPos = transform.localScale;
        endPos.x *= endPosition.x;
        endPos.y *= endPosition.y;
        endPos.z *= endPosition.z;
        Vector3 startPos = transform.localPosition;
        startPos.x *= fromPosition.x;
        startPos.y *= fromPosition.y;
        startPos.z *= fromPosition.z;

        transform.DOScale(endPos, duration).From(startPos).SetEase(ease).SetLoops(loops,loopType);
    }

    private void OnDisable()
    {
        transform.DOKill();
    }
}
