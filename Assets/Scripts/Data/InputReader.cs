using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(fileName = "InputReader", menuName = "Scriptable Objects/InputReader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    private Controls controls;
    private void OnEnable()
    {
        if (controls == null) 
        {
            controls = new Controls();
            controls.Player.RemoveCallbacks(this);
            controls.Player.AddCallbacks(this);
        }
    }
    public void EnableControls()
    {
        controls.Enable();
    }
    public void DisableControls()
    {
        controls.Disable();
    }

    private void OnDisable()
    {
        controls.Disable();
        controls.Player.RemoveCallbacks(this);
    }

    public event Action OnTouched;
    public void OnTouch(InputAction.CallbackContext context)
    {
        if (context.started) 
        {
            OnTouched?.Invoke();
        }
    }
}
