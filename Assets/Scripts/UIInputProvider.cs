using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ControlPanel
{
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
        [SerializeField] ToggleButton cuttersButton;
        [SerializeField] UIButton scaleUpButton;
        [SerializeField] UIButton scaleDownButton;
        [SerializeField] UIButton resetButton;

        private void Awake()
        {
            selectButton.onClick.AddListener(() => OnSelect?.Invoke());
            resetButton.onClick.AddListener(() => OnReset?.Invoke());
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

        public bool RotateToggle
        {
            get
            {
                return GetToggleActivated(rotateButton);
            }
        }
        public bool CutterToggle
        {
            get
            {
                return GetToggleActivated(cuttersButton);
            }
        }

        public bool Select
        {
            get
            {
                return GetButtonPressed(selectButton);
            }
        }
        public UnityEvent OnSelect;

        public bool Reset
        {
            get
            {
                return GetButtonPressed(resetButton);
            }
        }
        public UnityEvent OnReset;

        public float Scale
        {
            get
            {
                return ButtonToInt(scaleUpButton) - ButtonToInt(scaleDownButton);
            }
        }

        static int ButtonToInt(UIButton button)
        {
            if (!button)
                return 0;
            return button.isDown ? 1 : 0;
        }
        static bool GetButtonPressed(UIButton button)
        {
            if (!button)
                return false;
            return button.isDown;
        }
        static bool GetToggleActivated(ToggleButton toggle)
        {
            if (!toggle)
                return false;
            return toggle.Active;
        }
    }
}