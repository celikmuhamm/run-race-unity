using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGamePlatform:IPlatform
{
    public ObjectPool pool;
    public GameObject GetGameObj(PlatformConfig config,GameObject Last,float levelMultiplier)
    {
        InGameObject current;
        if (config.overrideObjectType)
        {
            current = pool.GetObject(config.name,config.directName);
        }
        else
        {
            current = pool.GetRandomObjectFromType(config.name);
        }
        
        if (Last.CompareTag("start"))
        {
            current.transform.position = Last.transform.GetChild(0).position + current.boundaries.positionOffsetVector;
        }
        else
        {
            current.transform.position = Last.transform.position + Last.GetComponent<InGameObject>().boundaries.endPosition + current.boundaries.positionOffsetVector;
        }
        current.gameObject.SetActive(true);
        current.enabled = true;
        current.levelMultiplier = levelMultiplier;
        return current.gameObject;
    }
}
