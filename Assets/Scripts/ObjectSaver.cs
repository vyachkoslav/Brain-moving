using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSaver
{
    class TransformValues
    {
        public Vector3 position, scale;
        public Quaternion rotation;
    }
    List<TransformValues> savedValues = new List<TransformValues>();
    List<Transform> savedObjects = new List<Transform>();

    public void Save(Transform obj)
    {
        if (savedObjects.Contains(obj))
        {
            DeleteSave(obj);
        }
        savedObjects.Add(obj);
        savedValues.Add(GetValues(obj));
    }
    public void Load(Transform obj)
    {
        if (savedObjects.Contains(obj))
        {
            int i = savedObjects.FindIndex(x => { return x == obj; });
            TransformValues values = savedValues[i];
            obj.localPosition = values.position;
            obj.localRotation = values.rotation;
            obj.localScale = values.scale;
        }
    }
    public void DeleteSave(Transform obj)
    {
        if (savedObjects.Contains(obj))
        {
            int i = savedObjects.FindIndex(x => { return x == obj; });
            savedObjects.RemoveAt(i);
            savedValues.RemoveAt(i);
        }
    }

    static TransformValues GetValues(Transform obj)
    {
        TransformValues values = new TransformValues
        {
            position = obj.localPosition,
            rotation = obj.localRotation,
            scale = obj.localScale
        };
        return values;
    }
}
