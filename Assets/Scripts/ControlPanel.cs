using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class ControlPanel : MonoBehaviour
{
    [SerializeField] ObjectGrabber grabber;

    [SerializeField] GrabbableObject grabbableParent;
    GrabbableObject selected;
    List<GrabbableObject> grabbables;
    int currentGrabbable;

    [SerializeField] ContainerArea container;

    [SerializeField] UIInputProvider input;
    [SerializeField] float speed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float scalingSpeed;

    public void SetGrabber(ObjectGrabber _grabber)
    {
        grabber = _grabber;
    }
    void Start()
    {
        grabbables = new List<GrabbableObject>(grabbableParent.GetComponentsInChildren<GrabbableObject>());

        selected = grabbables[currentGrabbable]; // parent

        input.OnSelect.AddListener(SelectNext);
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
            print(input.Scale * scalingSpeed * Time.deltaTime);
            selected.AddScale(input.Scale * scalingSpeed * Time.deltaTime);
        }
    }
    void SelectNext()
    {
        if (++currentGrabbable >= grabbables.Count)
        {
            currentGrabbable = 0;
            SelectAll(true);
        }
        else
            SelectAll(false);

        selected = grabbables[currentGrabbable];
    }
    void SelectAll(bool value)
    {
        var contained = new List<ContainedObject>(grabbableParent.GetComponentsInChildren<ContainedObject>());
        contained.ForEach(x => x.active = !value);
        contained[0].active = value;
        container.ResetArea();
    }
}
