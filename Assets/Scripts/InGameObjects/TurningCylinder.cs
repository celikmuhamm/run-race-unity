using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurningCylinder : InGameObject
{
    public float rotationSpeed = 0;
    public float impulsePower = 0;
    public float minSpeed = 10;
    public Vector3 rotationVector = new Vector3(0,1,0);
    public Vector3 centerOfMass;
    public Rigidbody turningBody;
    private Rigidbody _rigidBody = null;
    private bool _isRigidBodySet = false;

    public override void makeAction(Racer racer)
    {
        racer.Die();
    }
    
    void FixedUpdate()
    {
        if (isEnabled)
        {
            if (!_isRigidBodySet)
            {
                SetRigidBody( turningBody);
            }
            rotationSpeed = _rigidBody.angularVelocity.magnitude;
         
            if (rotationSpeed <= minSpeed*levelMultiplier)
            {
                AddTourque();
            }

            
        }
    }

    public void SetRigidBody(Rigidbody turningCylinderRigidBody)
    {
        _rigidBody = turningCylinderRigidBody;
        _rigidBody.centerOfMass = centerOfMass;
        _isRigidBodySet = true;
    }

    public void AddTourque()
    {
        _rigidBody.AddTorque(Time.deltaTime*rotationVector*impulsePower,ForceMode.Impulse);
    }
}
