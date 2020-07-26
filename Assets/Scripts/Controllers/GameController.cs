using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int playerLifeCount = 3;
    public int currentLifeCount = 3;
    public Racer player;
    public Racer[] aiPlayers;
    public Transform startPoint;
    public UIController uıController;
    void Start()
    {
     
       player.controller = this;
       Restart();
       
     
    }

    public void Restart()
    {
        InitUI();
        currentLifeCount = playerLifeCount;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
           player.Run();
        }
        else
        {
            player.Stop();
        }
    }

    public void CheckForRemainingLife()
    {
        if (currentLifeCount > 1)
        {
            currentLifeCount--;
            uıController.Updatehealth(currentLifeCount);
            player.ReSpawn(player.respawnPoint.position);
        }
        else
        {
            Restart();
            player.ReSpawn(startPoint.position);
        }
    }
    private void InitUI()
    {
        if (this.GetComponent<UIController>() != null)
        {
            uıController = this.GetComponent<UIController>();
        }
        else
        {
            throw new Exception("UIController instance not found on 'Controllers' object");
        }
        uıController.CreateHealthBar(playerLifeCount);
    }
    
    
}
