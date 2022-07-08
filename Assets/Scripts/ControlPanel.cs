using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace ControlPanel
{
    public class ControlPanel : MonoBehaviour
    {
        [SerializeField] GrabbableObject grabbableParent;
        List<GrabbableObject> grabbables;
        int currentGrabbable;
        GrabbableObject selected => grabbables[currentGrabbable];

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

        [SerializeField] Transform direction;

        readonly ObjectSaver saver = new ObjectSaver();

        void Start()
        {
            defaultScale = grabbableParent.transform.lossyScale.x;
            grabbables = new List<GrabbableObject>(grabbableParent.GetComponentsInChildren<GrabbableObject>());

            ResetTime();
            SelectAll(true);
            SaveGrabbables();

            input.OnSelect.AddListener(SelectNext);
            input.OnReset.AddListener(ResetGrabbables);
        }
        void Update()
        {
            HandleSleep();
            HandleInput();
        }
        void HandleSleep()
        {
            if (input.Direction == Vector3.zero && input.Scale == 0)
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
        }
        void HandleInput()
        {
            if (input.Direction != Vector3.zero)
            {
                if (input.RotateToggle)
                {
                    RotateSelected();
                }
                else
                {
                    MoveSelected();
                }
            }

            if (input.Scale != 0)
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
                value = direction.TransformDirection(value);
                selected.Move(value);
            }
            else
                MoveCutter();
        }
        void MoveCutter()
        {
            Vector3 value = Vector3.forward * input.Z * (speed * Time.deltaTime);
            value = cutterHorizontal.TransformDirection(value);

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
        }

        void SaveGrabbables()
        {
            grabbables.ForEach(x => saver.Save(x.transform));
        }
        void ResetGrabbables()
        {
            ResetTime();
            grabbables.ForEach(x => saver.Load(x.transform));
        }
        void ResetTime()
        {
            remainingTimeToSleep = secondsToSleep;
        }
    }
}