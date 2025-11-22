using Unity.VisualScripting;
using UnityEngine;

public class Gamemanager : Singleton<Gamemanager>
{
    [SerializeField] InputReader input;
    [SerializeField] EventSO eventSO;

    protected override void Awake()
    {
        base.Awake();
        eventSO.OnGameStarted += EventSO_OnGameStarted;
        eventSO.OnGameEnded += EventSO_OnGameEnded;
    }

    private void EventSO_OnGameEnded()
    {
        input.DisableControls();
    }

    private void EventSO_OnGameStarted()
    {
        input.EnableControls();
    }

    private void Start()
    {
        input.OnTouched += Input_OnTouched;
    }
    private void OnDisable()
    {
        input.OnTouched -= Input_OnTouched;
        eventSO.OnGameStarted -= EventSO_OnGameStarted;
        eventSO.OnGameEnded -= EventSO_OnGameEnded;
    }
    private void Input_OnTouched()
    {
        MovingCube.currentCube.Stop();
    }
    public void RaiseEventSO(int value)
    {
        eventSO.RaiseEvent(value);
    }
}
