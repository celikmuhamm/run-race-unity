using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class InGameObject: MonoBehaviour
{

    public bool isPrefabricated = false;
    public Transform prefabParent;
    public string type = "Unknown";
    public bool isActive = false;
    public bool isEnabled = true;
    
    public float levelMultiplier = 1;
    public ObjectBoundaries boundaries;

    public virtual void  makeAction(Racer racer)
    {
        Debug.Log("BaseClass");
    }
    public InGameObject getPrefabObject()
    {
        List<int> enabledAndInactiveInstances = new List<int>();
        for (int i = 0; i < prefabParent.transform.childCount;i++)
        {
            InGameObject cursor = prefabParent.GetChild(i).GetComponent<InGameObject>();
            if (!cursor.isActive && cursor.enabled)
            {
                enabledAndInactiveInstances.Add(i);
            }
        }
        if (enabledAndInactiveInstances.Count>0)
        {
            int selectedIndex = Random.Range(0, enabledAndInactiveInstances.Count);
            GameObject selectedObject = prefabParent.GetChild(enabledAndInactiveInstances[selectedIndex]).gameObject;
            selectedObject.SetActive(true);
            return selectedObject.GetComponent<InGameObject>();
        }
        return null;
    }
}