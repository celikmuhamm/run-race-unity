using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{

    public GameObject startPlatform;
    public Transform endPlatform;
    public Vector3 endPlatformOffset;
    public List<LevelConfig> levels;
    public int levelIndex;
    public LevelConfig currentLevel;
    public PlatformFactory factory;
    public List<GameObject> levelObjects;
    
    public void CreateLevel(int levelIndex)
    {
        this.levelIndex = levelIndex;
        if (levelIndex< levels.Count)
        {
            levelObjects = new List<GameObject>();
            currentLevel = levels[levelIndex];
            for(int i = 0;i< currentLevel.platforms.Count;i++)
            {
                PlatformConfig config = levels[levelIndex].platforms[i];
                IPlatform platform = factory.GetPlatform(config);
                if (i == 0)
                {
                    if (config.overrideLevelMultiplier)
                    {
                        levelObjects.Add(platform.GetGameObj(config,startPlatform,config.levelMultiplier));
                    }
                    else
                    {
                        levelObjects.Add(platform.GetGameObj(config,startPlatform,currentLevel.levelMultiplier));
                    }
                   
                }
                else
                {
                    if (config.overrideLevelMultiplier)
                    {
                        levelObjects.Add(platform.GetGameObj(config,levelObjects[i -1],config.levelMultiplier));
                    }
                    else
                    {
                        levelObjects.Add(platform.GetGameObj(config,levelObjects[i -1],currentLevel.levelMultiplier));
                    }
                }
            }

            Vector3 endPosition = factory.getEndingPosition(levelObjects[levelObjects.Count - 1]);
            endPlatform.position = endPosition + endPlatformOffset;
            endPlatform.gameObject.SetActive(true);
        }
        else
        {
            throw new Exception("Level count is: " + levels.Count + " You are passing level index: " + levelIndex);
        }
       
    }

    public void DeactivateLevel()
    {
        foreach (var levelObject in levelObjects)
        {
            levelObject.SetActive(false);
            factory.DeactivateObject(levelObject);
        }
    }
}
