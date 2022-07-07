using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ControlPanel
{
    public class ChangeButton : MonoBehaviour
    {
        [SerializeField] Image buttonImageComponent;
        [SerializeField] Color colorOn;
        [SerializeField] Color colorOff;
        public void SetColorOn()
        {
            buttonImageComponent.color = colorOn;
        }
        public void SetColorOff()
        {
            buttonImageComponent.color = colorOff;
        }
    }
}