using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ControlPanel
{
    public class GrabbableObject : MonoBehaviour
    {
        public void Move(Vector3 dir)
        {
            transform.position += dir;
        }
        public void Rotate(Vector3 euler)
        {
            transform.localEulerAngles += euler;
        }
        public void AddScale(float value)
        {
            transform.localScale += Vector3.one * value;
        }
        public void SetScale(float value)
        {
            transform.localScale = Vector3.one * value;
        }
    }
}