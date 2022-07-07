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

    [SerializeField] Transform cutterHorizontal;

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
        if (input.Direction == Vector3.zero && input.Scale == 0 && remainingTimeToSleep > 0)
            remainingTimeToSleep -= Time.deltaTime;
        else
            ResetTime();

        if (remainingTimeToSleep <= 0)
        {
            if (currentGrabbable != 0)
            {
                currentGrabbable = 0;
                selected = grabbables[currentGrabbable];
                SelectAll(true);
            }

            selected.Rotate(Vector3.up * sleepRotationSpeed * Time.deltaTime);
        }

        if (input.RotateToggle)
        {
            RotateSelected();
        }
        else
        {
            MoveSelected();
        }

        ScaleSelected();
    }

    void RotateSelected()
    {
        if (!input.CutterToggle)
        {
            Vector3 direction = new Vector3(input.Z, input.Y, input.X);
            Vector3 value = direction * rotationSpeed * Time.deltaTime;
            selected.Rotate(value);
        }
        else
            MoveCutter();
    }
    void MoveSelected()
    {
        if (!input.CutterToggle)
        {
            Vector3 value = input.Direction * (speed * Time.deltaTime);
            selected.Move(value);
        }
        else
            MoveCutter();
    }
    void MoveCutter()
    {
        Vector3 value = input.Direction * (speed * Time.deltaTime);
        value = Vector3.Scale(value, cutterHorizontal.forward);
        cutterHorizontal.position += value;
    }

    void ScaleSelected()
    {
        float value = input.Scale * scalingSpeed * Time.deltaTime;
        if (!input.CutterToggle)
            selected.AddScale(value);
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
        TransformValues values = new TransformValues
        {
            position = obj.localPosition,
            rotation = obj.localRotation,
            scale = obj.localScale
        };
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
