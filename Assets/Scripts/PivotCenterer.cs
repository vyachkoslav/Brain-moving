using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PivotCenterer : MonoBehaviour
{
    // sets pivot to center
    private void Reset()
    {
        Vector3 newPos = GetComponent<Renderer>().bounds.center;
        transform.position = newPos;
        transform.localPosition *= -1;
        transform.parent.position = newPos;
    }
}
