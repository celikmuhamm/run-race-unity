using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Todo : Object pool for InGameObject system.
 * @date : 4/13/2020
 * @author : Safa Celik
 */
 
public class ObjectPool : MonoBehaviour
{
    public GameObject objectParent;
    public GameObject instanceParent;
    public ObjectFactory factory;
    public List<string> objectGroupNames;
    public List<int> poolSizes;

    private List<MyQueue<InGameObject>> _pools;
    public void createObjectsAtStart()
    {
        factory.objectGroups = objectGroupNames;
        factory.setIngameObjects(objectParent);
        factory.instanceParent = instanceParent.transform;
        _pools = new List<MyQueue<InGameObject>>();
        for (int i = 0; i < objectGroupNames.Count; i++)
        {
          _pools.Add(new MyQueue<InGameObject>());
        }
        int total = 0;
        for (int i = 0;i<poolSizes.Count;i++)
        {
            total += poolSizes[i];
            if (i > 0)
            {
                poolSizes[i] += poolSizes[i - 1];
            }
        }
        int randomCountIndex = 0;
        for (int i = 0; i < total; i++)
        {
            InGameObject newObject = null;
            
            if (i< poolSizes[randomCountIndex])
            {
                newObject = factory.createRandomObject(objectGroupNames[randomCountIndex]);
            }
            else
            {
                randomCountIndex++;
                newObject = factory.createRandomObject(objectGroupNames[randomCountIndex]);
            }
            _pools[randomCountIndex].Enqueue(newObject);
        }
    }

    // Gets a random object from specified type. Simply dequeue from object type pool;
    public InGameObject GetRandomObjectFromType(string objectType)
    {
        int index = objectGroupNames.IndexOf(objectType);
        if (_pools[index].Count > 0)
        {
            InGameObject returnOBJ = _pools[index].Dequeue();
            returnOBJ.enabled = true;
            return returnOBJ;
        }
        else
        {
            throw new Exception("Please increase object pool size for" +objectType + " Object type . We run out of objects !!");
        }
    }

    /* Gets the exact object from specified type.
     * For example get "Collectible","BlueCoin" 
     * objectName: the type of the object.
     */ 
    public InGameObject GetObject(string objectType, string objectName)
    {
        int index = objectGroupNames.IndexOf(objectType);
        if (_pools[index].Count > 0)
        {
           
            foreach (var poolObject in _pools[index].Objects)
            {
                if (poolObject.GetType().ToString().Equals(objectName))
                {
                    InGameObject returnOBJ = poolObject;
                    _pools[index].RemoveObject(poolObject);
                    return returnOBJ;
                }
            }
            throw new Exception("Object with the class name: " +objectName + " Cannot be found in ObjectPool");
        
        }
        else
        {
            throw new Exception("Please increase object pool size for" +objectType + " Object type . We run out of objects !!");
        }
    }
    
    public void DeactivateObject(InGameObject gameObject)
    {
        int index = objectGroupNames.IndexOf(gameObject.type);
        gameObject.enabled = false;
        _pools[index].Enqueue(gameObject);
    }
}
