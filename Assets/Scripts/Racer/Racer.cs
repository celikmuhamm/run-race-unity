﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Todo : implement the Racer Ability for various projects.
 * This is the Character modified variation
 * variation Notes:
 *     The "isAI" bool is used for various actions and all of these actions can be removed from this class
 *     This is because for now there is only two roles under the racer "ai" or "player". If there will be more roles,
 *     then the actions using "isAI" bool should be transferred to more specific role classes along with the new roles.
 * @variationDate: 7/26/2020
 * @date : 4/20/2020
 * @author : Safa Celik
 */
 
public class Racer : MonoBehaviour
{
    public bool isAI = true;
    public float speed = 0;
    public float acceleration = 0;
    public GameController controller;
    public Transform respawnPoint;
    public float reSpawnTime;
    public GameObject hitCollider;
    public bool isActive = true;
    public Vector3 aiOffsetVector;
    
    /* since we now moving racers with blend tree,
     * there is no harm setting this value to "1".
     * However, we may want to change in future design or project
     */
    private float _maxSpeed = 1.0f;
    private float _minSpeed = 0.0f;
    private bool _speedUp = false;
    private Animator _animator = null;
    private Collider[] _childColliders;
    private Rigidbody[] _ragDollBodies;
    private DelayFunction _delayGenerator;
    private DelayFunction _materialDelay;
    private List<Material> _effectedMaterials;
    private Vector3[] _jointStartPositions;
    private Quaternion[] _jointStartRotations;
   
    private void Start()
    {
        _effectedMaterials = new List<Material>();
        SetChildColliders(this.GetComponentsInChildren<Collider>());
        SetChildBodies(this.GetComponentsInChildren<Rigidbody>());
        SetAnimator(this.GetComponent<Animator>());
        _delayGenerator = new DelayFunction();
        _materialDelay = new DelayFunction();
        SetStartPositions();
        if (!isAI)
        {
            ReSpawn(controller.startPoint.position);
        }
        else
        {
            Spawn();
        }
    }

    void FixedUpdate()
    {
        if (speed < _maxSpeed && _speedUp)
        {
            speed += acceleration * Time.deltaTime;
            _animator.SetFloat("Speed", speed);
        }
        else if (!_speedUp && speed > _minSpeed)
        {
            speed -= acceleration * Time.deltaTime;
            _animator.SetFloat("Speed", speed);
        }

        _delayGenerator.Update();
        _materialDelay.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InGameObject"))
        {
            hitCollider = other.gameObject;
            if (other.attachedRigidbody != null)
            {
                other.attachedRigidbody.transform.parent.GetComponent<InGameObject>().makeAction(this);
                if (!isAI)
                {
                    controller.SetCameraTarget(other.transform);
                    
                }
            }
            else
            {
                other.transform.parent.GetComponent<InGameObject>().makeAction(this);
                if (!isAI)
                {
                    controller.SetCameraTarget(other.transform);
                }
            }

            if (!isAI)
            {
                
                if (hitCollider.GetComponent<MeshRenderer>()!= null && 
                    hitCollider.GetComponent<MeshRenderer>().material.HasProperty("Boolean_DDA422C6"))
                {
                    Material effectMat = hitCollider.GetComponent<MeshRenderer>().material;
                    effectMat.SetFloat("Boolean_DDA422C6",1);
                    _effectedMaterials.Add(effectMat);
                    _materialDelay.Delay(SetMaterialToNormal,reSpawnTime);
                }
            }
            
        }

        else if (other.CompareTag("reSpawnPoint"))
        {
            respawnPoint = other.transform;
        }

        else if (other.CompareTag("end"))
        {
            Stop();
            isActive = false;
            if (!isAI)
            {
                controller.EndLevel();
            }
        }
    }

    public void Run()
    {
        if (isActive)
        {
            _speedUp = true;
        }
    }

    public void Stop()
    {
        _speedUp = false;
    }

    public void Die()
    {
        _animator.enabled = false;
        ActivateRigidBodies(true);
        this.GetComponent<Rigidbody>().isKinematic = true;
        ActivateColliders(true);
        this.GetComponent<Collider>().enabled = false;
        if (!isAI)
        {
            _delayGenerator.Delay(controller.CheckForRemainingLife, reSpawnTime);
        }
        else
        {
            _delayGenerator.Delay(ReSpawnAI, reSpawnTime);
        }
    }

    private void ReSpawnAI()
    {
        ReSpawn(respawnPoint.position);
    }

    public void ReSpawn(Vector3 position)
    {
        Spawn();
        if (isAI)
        {
            this.transform.position = position + aiOffsetVector;
        }
        else
        {
            this.transform.position = position;
        }
    }

    public void ActivateColliders(bool decision)
    {
        foreach (var collider in _childColliders)
        {
            collider.enabled = decision;
        }
    }

    public void ActivateRigidBodies(bool decision)
    {
        foreach (var rigidBody in _ragDollBodies)
        {
            rigidBody.isKinematic = !decision;
        }
    }

    public void SetAnimator(Animator racerAnimator)
    {
        _animator = racerAnimator;
    }

    public void SetChildColliders(Collider[] colliders)
    {
        _childColliders = colliders;
    }

    public void SetChildBodies(Rigidbody[] bodies)
    {
        _ragDollBodies = bodies;
    }

    public void SetStartPositions()
    {
        int childCount = _ragDollBodies.Length;
        _jointStartPositions = new Vector3[childCount];
        _jointStartRotations = new Quaternion[childCount];
        for (int i = 0; i < childCount; i++)
        {
            _jointStartPositions[i] = _ragDollBodies[i].transform.position;
            _jointStartRotations[i] = _ragDollBodies[i].transform.rotation;
        }
    }

    private void SetBodyToStart()
    {
        int childCount = _ragDollBodies.Length;

        for (int i = 0; i < childCount; i++)
        {
            _ragDollBodies[i].transform.position = _jointStartPositions[i];
            _ragDollBodies[i].transform.rotation = _jointStartRotations[i];
        }
    }

    private void Spawn()
    {
        _animator.applyRootMotion = true;
        _animator.enabled = true;
        _animator.Play("default");
        ActivateColliders(false);
        ActivateRigidBodies(false);
        this.GetComponent<Rigidbody>().isKinematic = false;
        this.GetComponent<Collider>().enabled = true;
        SetBodyToStart();
        isActive = true;
        if (!isAI)
        {
            controller.SetCameraTarget(this.transform);
        }
    }

    private void SetMaterialToNormal()
    {
        foreach (var material in _effectedMaterials)
        {
            material.SetFloat("Boolean_DDA422C6",0);
        }
        _effectedMaterials = new List<Material>();
    }
}