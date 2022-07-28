using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace ControlPanel
{
    /// <summary>
    /// Doesn't pass objects out of its collider
    /// </summary>
    public class ContainerArea : MonoBehaviour
    {
        [SerializeField] float maxOutDistance;

        private void Update()
        {
            ClampPositions();
        }
        private void ClampPositions()
        {
            var children = GetChildren();

            for (int i = 0; i < children.Count; ++i)
            {
                if (children[i].active)
                {
                    size++;
                    Transform child = children[i].transform;

                    Vector3 posOnCollider = GetComponent<Collider>().ClosestPoint(child.position);
                    if (OutOfArea(child.position))
                    {
                        child.position = posOnCollider;
                    }
                }
            }
        }
        List<ContainedObject> GetChildren()
        {
            var children = new List<ContainedObject>(gameObject.GetComponentsInChildren<ContainedObject>());
            children = children.FindAll(x => { return x.active; });
            return children;
        }
        bool OutOfArea(Vector3 position)
        {
            Vector3 posOnCollider = GetComponent<Collider>().ClosestPoint(position);
            return Vector3.Distance(position, posOnCollider) > maxOutDistance;
        }
    }
}