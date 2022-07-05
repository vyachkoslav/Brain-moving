using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIInputProvider : MonoBehaviour
{
    [SerializeField] UIButton forwardButton;
    [SerializeField] UIButton backwardButton;
    [SerializeField] UIButton upButton;
    [SerializeField] UIButton downButton;
    [SerializeField] UIButton rightButton;
    [SerializeField] UIButton leftButton;
    [SerializeField] UIButton selectButton;
    [SerializeField] ToggleButton rotateButton;
    [SerializeField] UIButton scaleUpButton;
    [SerializeField] UIButton scaleDownButton;

    private void Awake()
    {
        selectButton.onClick.AddListener(() => OnSelect?.Invoke());
    }

    static int ButtonToInt(UIButton button)
    {
        if (!button)
            return 0;
        return button.isDown ? 1 : 0;
    }
    public float Z
    {
        get
        {
            return ButtonToInt(forwardButton) - ButtonToInt(backwardButton);
        }
    }
    public float X
    {
        get
        {
            return ButtonToInt(rightButton) - ButtonToInt(leftButton);
        }
    }
    public float Y
    {
        get
        {
            return ButtonToInt(upButton) - ButtonToInt(downButton);
        }
    }
    public Vector3 Direction
    {
        get
        {
            return new Vector3(X, Y, Z);
        }
    }

    public bool ControlModifier
    {
        get
        {
            return rotateButton.Active;
        }
    }

    public bool Select
    {
        get
        {
            return selectButton.isDown;
        }
    }
    public UnityEvent OnSelect;

    public float Scale
    {
        get
        {
            return ButtonToInt(scaleUpButton) - ButtonToInt(scaleDownButton);
        }
    }
}
