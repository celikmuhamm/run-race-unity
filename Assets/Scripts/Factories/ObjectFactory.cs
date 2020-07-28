using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ObjectFactory 
{
    public static ObjectFactory instance;
    public GameObject ingameObjectParent;
    public List<string> objectGroups;
    public Transform instanceParent;
    private List<List<InGameObject>> objects;


 
    public static ObjectFactory GetInstance()
    {
        if (instance == null)
        {
            instance = new ObjectFactory();
        }
        return instance;
    }
    public void setIngameObjects(GameObject parentObject)
    {
        ingameObjectParent = parentObject;
        objects = new List<List<InGameObject>>();
        for (int i = 0; i< objectGroups.Count;i++)
        {
            objects.Add(new List<InGameObject>());
        }

        for (int i = 0; i < parentObject.transform.childCount; i++)
        {
            if ( parentObject.transform.GetChild(i).GetComponent<InGameObject>()!=null)
            {
                InGameObject cursor = parentObject.transform.GetChild(i).GetComponent<InGameObject>();
                cursor.boundaries = ingameObjectParent.GetComponent<BoundingBoxCalculator>().platformBounds[i];
                if (objectGroups.Contains(cursor.type))
                {
                    objects[objectGroups.IndexOf(cursor.type)].Add(cursor);
                }
            }
            

        }
    }
    public InGameObject createRandomObject( string objectType)
    {
        if (!objectGroups.Contains(objectType) ||objects[objectGroups.IndexOf(objectType)].Count == 0)
        {
            return null;
        }

        int groupIndex = objectGroups.IndexOf(objectType);
        InGameObject selectedObject = null;
        int randomIndex = Random.Range(0, objects[groupIndex].Count);
        selectedObject = objects[groupIndex][randomIndex];

        InGameObject newObject = null;
        if (!selectedObject.isPrefabricated)
        {
            newObject = GameObject.Instantiate(selectedObject);
            newObject.transform.localScale = selectedObject.transform.localScale;
            newObject.transform.SetParent(instanceParent);
            return newObject;
        }
        newObject = selectedObject.getPrefabObject();
        return newObject;


    }
}
