using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class ControlPanel : MonoBehaviour
{
    [SerializeField] ObjectGrabber grabber;
    [SerializeField] GrabbableObject parent;
    GrabbableObject selected;
    [SerializeField] ContainerArea container;

    [SerializeField] UIInputProvider input;
    [SerializeField] float speed;
    [SerializeField] float rotationSpeed;

    public void SetGrabber(ObjectGrabber _grabber)
    {
        grabber = _grabber;
    }
    void Start()
    {
        selected = parent;
        input.OnSelect.AddListener(() => SelectAll(true));
    }
    void Update()
    {
        if (selected)
        {
            if (input.ControlModifier)
            {
                Vector3 direction = new Vector3(input.Z, input.Y, input.X);
                selected.Rotate(direction * rotationSpeed * Time.deltaTime);
            }
            else
            {
                selected.Move(input.Direction * speed * Time.deltaTime);
            }
        }
    }
    void SelectAll(bool value)
    {
        var contained = new List<ContainedObject>(parent.GetComponentsInChildren<ContainedObject>());
        contained.ForEach(x => x.active = !value);
        parent.GetComponent<ContainedObject>().active = value;
        container.ResetArea();
    }
    void XRSelectAll(bool value)
    {
        SelectAll(value);
        var XRGrabbable = new List<XRGrabInteractable>(parent.GetComponentsInChildren<XRGrabInteractable>());
        XRGrabbable.ForEach(x => x.enabled = !value);
        parent.GetComponent<XRGrabInteractable>().enabled = value;
    }
}
