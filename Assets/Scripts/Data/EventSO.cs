using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EventSO", menuName = "Scriptable Objects/EventSO")]
public class EventSO : ScriptableObject
{
    public event Action OnGameStarted;
    public event Action OnGameEnded;
    public event Action OnSpawnRequest;
    public event Action OnScoreIncrement;

    public void RaiseEvent(int value)
    {
        switch (value)
        {
            case 0:
                OnGameStarted?.Invoke();
                break;
            case 1:
                OnGameEnded?.Invoke();
                break;
            case 2:
                OnSpawnRequest?.Invoke();
                break;
            case 3:
                OnScoreIncrement?.Invoke();
                break;
            default:
                break;
        }
    }
}
