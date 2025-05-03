using System;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    public bool AlwaysPerformCheck = true;

    public Vector3 boxSize = new Vector3(5, 5, 5);      
    public Vector3 boxOffset = Vector3.zero;            
    public LayerMask triggerLayer;

    public List<GameObject> currentObjects = new List<GameObject>();

    public Action<GameObject> onTriggerEnter;
    public Action<GameObject> onTriggerExit;
    public Action<GameObject> onTriggerStay;

    void Update()
    {
        if(AlwaysPerformCheck)
            PerformBoxCheck();
    }

    //can be used as always update or call perform check to get results;
    public List<GameObject> PerformBoxCheck()
    {
        Vector3 center = transform.position + transform.rotation * boxOffset;
        Collider[] hits = Physics.OverlapBox(center, boxSize * 0.5f, transform.rotation, triggerLayer);
        List<GameObject> newObjects = new List<GameObject>();

        //Handle Enter and Stay
        foreach (var hit in hits)
        {
            GameObject obj = hit.gameObject;
            newObjects.Add(obj);

            if (!currentObjects.Contains(obj))
            {
                OnObjectEnter(obj);
                currentObjects.Add(obj);
                onTriggerEnter?.Invoke(obj);
            }
            else
            {
                OnObjectStay(obj);
                onTriggerStay?.Invoke(obj);
            }
        }

        //Handle Exit
        for (int i = 0; i < currentObjects.Count; i++) { 
            var obj = currentObjects[i];
            if (!newObjects.Contains(obj))
            {
                OnObjectExit(obj);
                currentObjects.Remove(obj);
                i--;
                onTriggerExit?.Invoke(obj);
            }
        }
        return newObjects;
    }

    private void OnObjectEnter(GameObject obj)
    {
        
    }

    private void OnObjectStay(GameObject obj)
    {
        
    }

    private void OnObjectExit(GameObject obj)
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.25f);
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position + transform.rotation * boxOffset, transform.rotation, boxSize);
        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }
}
