using UnityEngine;

public class LeanScale : MonoBehaviour
{

    [SerializeField]
    float scaleDuration=0.50f;
    public void ScaleAnimation()
    {
        LeanTween.scale(this.gameObject, Vector3.one * 1.1f, scaleDuration).setLoopPingPong();
    }

    private void OnDisable()
    {
        LeanTween.cancel(this.gameObject);
    }
}
