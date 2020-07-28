using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class LevelConfig
{
    public string name;
    public List<PlatformConfig> platforms;
    public float levelMultiplier = 1;

    public LevelConfig()
    {
        name = "level";
    }
}

