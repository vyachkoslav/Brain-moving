using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRRayInteractorDecorator : XRRayInteractor
{
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);

        if (!useForceGrab && interactablesSelected.Count == 1 && TryGetCurrent3DRaycastHit(out var raycastHit))
            attachTransform.position = raycastHit.collider.attachedRigidbody.position;
    }
}
