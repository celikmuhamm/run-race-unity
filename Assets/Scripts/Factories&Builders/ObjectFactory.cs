using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Todo : Object Factory for InGameObject system
 * This is the Desired Object modified variation
 * variation Notes:
 *     Since the level configuration has the ability to demand specified object,
 *     factory should ensure that each object type has minimum amount of instance.
 * @date : 4/13/2020
 * @author : Safa Celik
 */
 
public sealed class ObjectFactory
{
    public static ObjectFactory instance;
    public GameObject ingameObjectParent;
    public List<string> objectGroups;
    public Transform instanceParent;
    public int minObjectCount;
    private List<List<InGameObject>> objects;
    private List<List<int>> _objectCounts;

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
        _objectCounts = new List<List<int>>();
        for (int i = 0; i < objectGroups.Count; i++)
        {
            objects.Add(new List<InGameObject>());
            _objectCounts.Add(new List<int>());
        }

        for (int i = 0; i < parentObject.transform.childCount; i++)
        {
            if (parentObject.transform.GetChild(i).GetComponent<InGameObject>() != null)
            {
                InGameObject cursor = parentObject.transform.GetChild(i).GetComponent<InGameObject>();
                cursor.boundaries = ingameObjectParent.GetComponent<BoundingBoxCalculator>().platformBounds[i];
                if (objectGroups.Contains(cursor.type))
                {
                    objects[objectGroups.IndexOf(cursor.type)].Add(cursor);
                    _objectCounts[objectGroups.IndexOf(cursor.type)].Add(0);
                }
            }
        }
    }

    public InGameObject createRandomObject(string objectType)
    {
        if (!objectGroups.Contains(objectType) || objects[objectGroups.IndexOf(objectType)].Count == 0)
        {
            return null;
        }

        int groupIndex = objectGroups.IndexOf(objectType);
        InGameObject selectedObject = null;
        int randomIndex = Random.Range(0, objects[groupIndex].Count);
        
        /*
         * this part ensures the minimum instance creation of each object type
         */
        bool minCreated = true;
        foreach (var size in _objectCounts[groupIndex])
        {
            if (size < minObjectCount)
            {
                minCreated = false;
                break;
            }
        }

       
        if (!minCreated)
        {
            List<int> randomList = new List<int>();
            for (int i = 0; i < _objectCounts[groupIndex].Count; i++)
            {
                if (_objectCounts[groupIndex][i] < minObjectCount)
                {
                        randomList.Add(i);
                }
            }
            randomIndex = randomList[Random.Range(0, randomList.Count)];
        }
        selectedObject = objects[groupIndex][randomIndex];
        _objectCounts[groupIndex][randomIndex]++;
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