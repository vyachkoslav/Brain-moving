using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class ControlPanel : MonoBehaviour
{
    class TransformValues
    {
        public Vector3 position, scale;
        public Quaternion rotation;
    }
    [SerializeField] ObjectGrabber grabber;

    [SerializeField] GrabbableObject grabbableParent;
    GrabbableObject selected;
    List<GrabbableObject> grabbables;
    List<TransformValues> defaultValues;
    int currentGrabbable;

    [SerializeField] ContainerArea container;

    [SerializeField] UIInputProvider input;
    [SerializeField] float speed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float scalingSpeed;

    [SerializeField] float secondsToSleep;
    float remainingTimeToSleep;
    [SerializeField] float sleepRotationSpeed;

    public void SetGrabber(ObjectGrabber _grabber)
    {
        grabber = _grabber;
    }
    void Start()
    {
        ResetTime();

        defaultValues = new List<TransformValues>();
        grabbables = new List<GrabbableObject>(grabbableParent.GetComponentsInChildren<GrabbableObject>());

        selected = grabbables[currentGrabbable]; // parent
        SelectAll(true);

        SaveGrabbables();
       
        input.OnSelect.AddListener(SelectNext);
        input.OnReset.AddListener(ResetGrabbables);
    }
    void Update()
    {
        if (input.Direction == Vector3.zero && input.Scale == 0)
            remainingTimeToSleep -= Time.deltaTime;
        else
            ResetTime();

        if(remainingTimeToSleep <= 0)
        {
            if(currentGrabbable != 0)
            {
                currentGrabbable = 0;
                selected = grabbables[currentGrabbable];
                SelectAll(true);
            }

            selected.Rotate(Vector3.up * sleepRotationSpeed * Time.deltaTime);
        }

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

            selected.AddScale(input.Scale * scalingSpeed * Time.deltaTime);
        }
    }
    void SelectNext()
    {
        ResetTime();
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
    
    void SaveGrabbables()
    {
        defaultValues.Clear();
        foreach (GrabbableObject grabbable in grabbables)
        {
            defaultValues.Add(GetValues(grabbable.transform));
        }
    }
    void ResetGrabbables()
    {
        ResetTime();
        for (int i = 0; i < defaultValues.Count; ++i)
        {
            FillValues(grabbables[i].transform, defaultValues[i]);
        }
    }
    TransformValues GetValues(Transform obj)
    {
        TransformValues values = new TransformValues();
        values.position = obj.localPosition;
        values.rotation = obj.localRotation;
        values.scale = obj.localScale;
        return values;
    }
    void FillValues(Transform obj, TransformValues values)
    {
        obj.localPosition = values.position;
        obj.localRotation = values.rotation;
        obj.localScale = values.scale;
    }
    void ResetTime()
    {
        remainingTimeToSleep = secondsToSleep;
    }
}
