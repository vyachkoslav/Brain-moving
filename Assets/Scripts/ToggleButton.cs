using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToggleButton : MonoBehaviour
{
    public bool Active { get; private set; }
    public void Toggle()
    {
        Active = !Active;
        InvokeEvents();
    }
    void InvokeEvents()
    {
        OnToggle?.Invoke(Active);

        if(Active)
            OnTurnedOn?.Invoke();
        else
            OnTurnedOff?.Invoke();
    }

    public UnityEvent<bool> OnToggle;
    public UnityEvent OnTurnedOn;
    public UnityEvent OnTurnedOff;
}
