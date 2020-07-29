using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;

public class GameController : MonoBehaviour
{
    public int playerLifeCount = 3;
    public int currentLifeCount = 3;
    public int levelIndex = 0;
    public Racer player;
    public Racer[] aiPlayers;
    public Transform startPoint;
    public UIController uiController;
    public LevelController levelController;
    public ObjectPool pool;
    public Transform cameraRig;
    public float racerSpeed = 0.8f;
    private Vector3 cameraStartPosition;
    void Start()
    { 
        cameraStartPosition = cameraRig.position;
       player.controller = this;
       Restart();
       CreatePlatformFactory();
       CreateObjectFactory();
       pool.createObjectsAtStart();
       levelController.CreateLevel(levelIndex);
       foreach (var racer in aiPlayers)
       {
           racer.GetComponent<Animator>().speed = racerSpeed * levelController.currentLevel.levelMultiplier;
       }
        
    }

    public void Restart()
    {
        InitUI();
        currentLifeCount = playerLifeCount;
        cameraRig.transform.position = cameraStartPosition;
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
            uiController.Updatehealth(currentLifeCount);
            player.ReSpawn(player.respawnPoint.position);
        }
        else
        {
            Restart();
            player.ReSpawn(startPoint.position);
        }
    }

    public void EndLevel()
    {
        levelIndex++;
        if (levelIndex < levelController.levels.Count)
        {
            levelController.DeactivateLevel();
            levelController.CreateLevel(levelIndex);
            Restart();
            player.respawnPoint = startPoint;
            player.ReSpawn(startPoint.position);
        }
        else
        {
            levelIndex--;
            levelController.DeactivateLevel();
            levelController.CreateLevel(levelIndex);
            Restart();
            player.respawnPoint = startPoint;
            player.ReSpawn(startPoint.position);
        }

        foreach (var racer in aiPlayers)
        {
            racer.respawnPoint = startPoint;
            racer.ReSpawn(startPoint.position);
        }
        
    }

    public void SetCameraTarget(Transform newTarget)
    {
        cameraRig.GetComponent<AutoCam>().SetTarget( newTarget);
    }
    private void InitUI()
    {
        if (this.GetComponent<UIController>() != null)
        {
            uiController = this.GetComponent<UIController>();
        }
        else
        {
            throw new Exception("UIController instance not found on 'Controllers' object");
        }
        uiController.CreateHealthBar(playerLifeCount);
        uiController.SetLevelText(levelController.levels[levelIndex].name);
    }

    private void CreatePlatformFactory()
    {
        PlatformFactory factory = PlatformFactory.GetInstance();
        factory.pool = pool;
        levelController.factory = factory;
    }

    private void CreateObjectFactory()
    {
        ObjectFactory factory = ObjectFactory.GetInstance();
        pool.factory = factory;
        
    }
    
    
}
