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

    [SerializeField] GrabbableObject grabbableParent;
    GrabbableObject selected => grabbables[currentGrabbable];
    List<GrabbableObject> grabbables;
    List<TransformValues> defaultValues;
    int currentGrabbable;

    [SerializeField] ContainerArea container;

    [SerializeField] UIInputProvider input;
    [SerializeField] float speed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float scalingSpeed;
    [SerializeField] float maxScale;
    float defaultScale;

    [SerializeField] float secondsToSleep;
    float remainingTimeToSleep;
    [SerializeField] float sleepRotationSpeed;

    [SerializeField] Transform cutterHorizontal;

    readonly Vector3 k_invertor = new Vector3(-1, 1, -1);
    
    void Start()
    {
        ResetTime();

        defaultValues = new List<TransformValues>();
        defaultScale = grabbableParent.transform.lossyScale.x;

        grabbables = new List<GrabbableObject>(grabbableParent.GetComponentsInChildren<GrabbableObject>());

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

        if(input.Scale != 0)
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
            Vector3 value = Vector3.Scale(k_invertor, input.Direction) * (speed * Time.deltaTime);
            selected.Move(value);
        }
        else
            MoveCutter();
    }
    void MoveCutter()
    {
        Vector3 value = Vector3.Scale(k_invertor, input.Direction) * (speed * Time.deltaTime);
        value = Vector3.Scale(value, cutterHorizontal.forward);
        cutterHorizontal.position += value;
    }

    void ScaleSelected()
    {
        if (!input.CutterToggle)
        {
            float currentScale = selected.transform.lossyScale.x;

            float value = input.Scale * (scalingSpeed * Time.deltaTime);
            value = Mathf.Clamp(value + currentScale, defaultScale - maxScale, defaultScale + maxScale);

            SetGlobalScale(selected, value);
        }
    }
    static void SetGlobalScale(GrabbableObject obj, float value)
    {
        obj.SetScale(1);
        obj.SetScale(value / obj.transform.lossyScale.x);
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
