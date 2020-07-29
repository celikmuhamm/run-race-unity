using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlatformFactory
{
    public ObjectPool pool;
    public static PlatformFactory instance;
    private InGamePlatform _inGamePlatform;
    public static PlatformFactory GetInstance()
    {
        if (instance == null)
        {
            instance = new PlatformFactory();
        }
        return instance;
    }

    public IPlatform GetPlatform(PlatformConfig config)
    {
       
        if (pool.objectGroupNames.Contains(config.name))
        {
            if (_inGamePlatform == null)
            {
                _inGamePlatform = new InGamePlatform();
            }
            _inGamePlatform.pool = pool;
            return _inGamePlatform;
        }
        else
        {
            throw new Exception("There is no InGameObject with name: " + config.name);
        }
    }

    public Vector3 getEndingPosition(GameObject last)
    {
        return last.GetComponent<InGameObject>().boundaries.endPosition + last.transform.position;
    }

    public void DeactivateObject(GameObject levelObject)
    {

            if (levelObject.GetComponent<InGameObject>() != null)
            {
                pool.DeactivateObject(levelObject.GetComponent<InGameObject>());
            }
        
    }
}
