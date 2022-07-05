using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    public void Move(Vector3 pos)
    {
        transform.position += pos;
    }
    public void Rotate(Vector3 euler)
    {
        transform.eulerAngles += euler;
    }
}
