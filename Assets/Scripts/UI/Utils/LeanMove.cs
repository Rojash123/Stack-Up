using UnityEngine;
using UnityEngine.Events;

public class LeanMove : MonoBehaviour
{
    [SerializeField]
    public Transform objectToMove;

    [SerializeField]
    public Transform initialTransform;

    [SerializeField]
    public Transform finalTransform;

    [SerializeField] float leanDuration=0.25f;

    [SerializeField] LeanTweenType tweenType;

    private void OnEnable()
    {
        objectToMove.transform.position = initialTransform.position;
        LeanTween.move(objectToMove.gameObject, finalTransform.position, leanDuration).setEase(tweenType).setOnComplete(() =>
        {
            LeanScale scale =GetComponent<LeanScale>();
            if (scale != null) 
            {
                scale.ScaleAnimation();
            }
        });
    }

    private void OnDisable()
    {
        LeanTween.cancel(this.gameObject);
    }
}
