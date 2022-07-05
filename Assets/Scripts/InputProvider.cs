using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputProvider : MonoBehaviour
{
    static PlayerInputActions actions;
    
    private void Awake()
    {
        actions = new PlayerInputActions();
        actions.Enable();

        actions.Player.Toggle.started += (x) => OnToggle?.Invoke(x);
        actions.Player.Fire.started += (x) => OnFire?.Invoke(x);
    }
    public static System.Action<InputAction.CallbackContext> FuncToCallback<T>(System.Func<T> action)
    {
        return (InputAction.CallbackContext x) => action();
    }
    
    public static float Z
    {
        get
        {
            return actions.Player.Z.ReadValue<float>();
        }
    }
    public static float X
    {
        get
        {
            return actions.Player.X.ReadValue<float>();
        }
    }
    public static float Y
    {
        get
        {
            return actions.Player.Y.ReadValue<float>(); ;
        }
    }
    public static Vector3 Direction
    {
        get
        {
            return new Vector3(X, Y, Z);
        }
    }

    public static bool Fire
    {
        get
        {
            return actions.Player.Fire.ReadValue<float>() != 0;
        }
    }
    public static System.Action<InputAction.CallbackContext> OnFire;

    public static bool Toggle
    {
        get
        {
            return actions.Player.Toggle.ReadValue<float>() != 0;
        }
    }
    public static System.Action<InputAction.CallbackContext> OnToggle;

    public static bool ControlModifier
    {
        get
        {
            return actions.Player.ControlModifier.ReadValue<float>() != 0;
        }
    }
}
