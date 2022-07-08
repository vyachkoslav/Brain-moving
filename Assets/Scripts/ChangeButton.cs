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
        [SerializeField] List<Button> buttonsToTurn;

        public void SetColorOn()
        {
            buttonImageComponent.color = colorOn;
        }
        public void SetColorOff()
        {
            buttonImageComponent.color = colorOff;
        }
        public void TurnOnButtons()
        {
            buttonsToTurn.ForEach(x => x.interactable = true);
        }
        public void TurnOffButtons()
        {
            buttonsToTurn.ForEach(x => x.interactable = false);
        }
    }
}