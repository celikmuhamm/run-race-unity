using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public Transform healthGroup;

    private Transform[] healthObjects;
    public void CreateHealthBar(int healthCount)
    {
        
        int maxHeath = healthGroup.childCount;
        if (healthCount > maxHeath)
        {
            throw new Exception("Max allowed health count is: " + maxHeath);
        }
        else
        {
            healthObjects = new Transform[healthCount];
            for(int i = 0; i< healthCount;i++)
            {
                healthObjects[i] = healthGroup.GetChild(i);
                healthObjects[i].gameObject.SetActive(true);
            }
        }
    }

    public void Updatehealth(int healthCount)
    {
        healthObjects[healthCount].gameObject.SetActive(false);
    }
}
