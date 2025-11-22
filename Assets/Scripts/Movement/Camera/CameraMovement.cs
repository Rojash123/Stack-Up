using System.Threading;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] EventSO EventSO;
    int count = 0;
    bool canMoveCamera;

    private float targetY=5.6f;
    private float velocity = 0f;

    private void Awake()
    {
        EventSO.OnScoreIncrement += EventSO_OnScoreIncrement;
    }
    private void OnDestroy()
    {
        EventSO.OnScoreIncrement -= EventSO_OnScoreIncrement;
    }

    private void EventSO_OnScoreIncrement()
    {
        count++;
        if(count > 5)
        {
            targetY += 0.25f;
            canMoveCamera = true;
        }
    }

    private void LateUpdate()
    {
        if (!canMoveCamera) return;
        float newY = Mathf.SmoothDamp(transform.position.y, targetY, ref velocity, 0.25f);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
