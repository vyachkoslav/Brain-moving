using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ContainerArea : MonoBehaviour
{
    [SerializeField] float maxOutDistance;
    List<ContainedObject> children;
    List<Transform> parents;

    private void Start()
    {
        ResetArea();
    }

    /// <summary>
    /// Sets all children with rigidbody to not leave the container
    /// </summary>
    public void ResetArea()
    {
        children = new List<ContainedObject>(gameObject.GetComponentsInChildren<ContainedObject>());
        children = children.FindAll(x => { return x.active; });

        parents = new List<Transform>();
        foreach (ContainedObject child in children)
        {
            parents.Add(child.transform.parent);
        }
        Application.onBeforeRender += ClampPositions;
    }
    private void ClampPositions()
    {
        for(int i = 0; i < children.Count; ++i)
        {
            Transform child = children[i].transform;
            Transform parent = parents[i];

            Vector3 posOnCollider = GetComponent<Collider>().ClosestPoint(child.position);
            if (OutOfArea(child.position))
            {
                child.position = posOnCollider;
                child.SetParent(parent);
                parent.gameObject.SetActive(false);
                parent.gameObject.SetActive(true);
            }
        }
    }
    bool OutOfArea(Vector3 position)
    {
        Vector3 posOnCollider = GetComponent<Collider>().ClosestPoint(position);
        return Vector3.Distance(position, posOnCollider) > maxOutDistance;
    }
}
