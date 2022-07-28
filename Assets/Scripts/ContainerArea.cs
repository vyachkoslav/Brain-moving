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
        List<ContainedObject> allChildren;

        void Start()
        {
            allChildren = new List<ContainedObject>(gameObject.GetComponentsInChildren<ContainedObject>());
        }
        private void Update()
        {
            ClampPositions();
        }
        private void ClampPositions()
        {
            var children = GetContained();

            for (int i = 0; i < children.Count; ++i)
            {
                if (children[i].active)
                {
                    Transform child = children[i].transform;

                    Vector3 posOnCollider = GetComponent<Collider>().ClosestPoint(child.position);
                    if (OutOfArea(child.position))
                    {
                        child.position = posOnCollider;
                    }
                }
            }
        }
        List<ContainedObject> GetContained()
        {
            return allChildren.FindAll(x => { return x.active; }); ;
        }
        bool OutOfArea(Vector3 position)
        {
            Vector3 posOnCollider = GetComponent<Collider>().ClosestPoint(position);
            return Vector3.Distance(position, posOnCollider) > maxOutDistance;
        }
    }
}