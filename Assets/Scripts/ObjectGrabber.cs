using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectGrabber : MonoBehaviour
{
    [SerializeField] float raycastDistance;
    [SerializeField] int grababbleLayer;
    [SerializeField] float speed;
    [SerializeField] float rotationSpeed;
    [SerializeField] FlyCamera controls; // to change

    GrabbableObject grabbed;
    GrabbableObject hovered;
    Material hoveredDefaultMat;
    [SerializeField] Material hoverMaterial;

    enum ControlType
    {
        Translation,
        Rotation
    }
    static ControlType controlType = ControlType.Translation;

    public static void ToggleControls(bool value)
    {
        controlType = value ? ControlType.Rotation : ControlType.Translation;

        print(controlType);
    }

    void Hover()
    {
        if (hovered)
        {
            hovered.GetComponentInChildren<Renderer>().material = hoveredDefaultMat;
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance, grababbleLayer))
        {
            hovered = hit.transform.GetComponent<GrabbableObject>() ??
                      hit.transform.GetComponentInParent<GrabbableObject>();

            if (hovered)
            {
                Renderer hoveredRenderer = hovered.GetComponentInChildren<Renderer>();
                hoveredDefaultMat = hoveredRenderer.material;
                hoveredRenderer.material = hoverMaterial;
            }
        }
        else if (hovered)
        {
            hovered.GetComponentInChildren<Renderer>().material = hoveredDefaultMat;
        }
    }

    public bool TryGrab()
    {
        if (hovered)
        {
            Select(hovered);
            return true;
        }
        return false;
    }
    public void Select(GrabbableObject grabbable)
    {
        grabbed = grabbable;
    }

    void LockControls()
    {
        controls.lockPosition = true;
    }
    void UnlockControls()
    {
        controls.lockPosition = false;
    }

    private void Start()
    {
        InputProvider.OnFire += InputProvider.FuncToCallback(TryGrab);
    }
    private void Update()
    {
        Hover();

        if (grabbed)
        {
            LockControls();

            if(!InputProvider.ControlModifier)
                grabbed.Move(InputProvider.Direction * speed * Time.deltaTime);
            else
            {
                Vector3 direction = new Vector3(InputProvider.Z, InputProvider.Y, InputProvider.X);
                grabbed.Rotate(direction * rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            UnlockControls();
        }
    }
}
