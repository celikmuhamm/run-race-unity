using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidePress : InGameObject
{
    public float rewindTime = 3.0f;
    public float forceTime = 1.5f;
    public float forceAmount = 40;
    public Vector3 reverseSpeed;
    public Transform leftPress;
    public Transform rightPress;
    public float sideDistance = 4;
    private bool _rightRewinded = false;
    private bool _leftRewinded = false;

    private float rewindTimer = 0;
    
    private float forceTimer = 0;

    private void Start()
    {
        rewindTimer = Time.time + rewindTime;
    }

    private void FixedUpdate()
    {
        if (Time.time < rewindTimer )
        {
            if (!_rightRewinded)
            {
                rewindPress(rightPress,1);
            }
            if (!_leftRewinded)
            {
                rewindPress(leftPress,-1);
            }
           
        }
        else if(forceTimer< Time.time && !_leftRewinded&&!_rightRewinded)
        {
            rewindTimer = Time.time + rewindTime;
            
        }else if (forceTimer< Time.time&&_rightRewinded && _leftRewinded)
        {
            forceTimer = Time.time + forceTime;
            leftPress.GetComponent<Rigidbody>().velocity =  forceAmount*-1*levelMultiplier*-reverseSpeed;
            rightPress.GetComponent<Rigidbody>().velocity =  forceAmount*levelMultiplier*-reverseSpeed;
            _rightRewinded = false;
            _leftRewinded = false;
        }
    }

    public override void makeAction(Racer racer)
    {
        racer.Die();
    }

    private void rewindPress(Transform press,int side)
    {
        if (Math.Abs(press.position.x) < sideDistance)
        {
            press.GetComponent<Rigidbody>().velocity =  reverseSpeed*side;
        }
        else
        {
            press.GetComponent<Rigidbody>().velocity = Vector3.zero;
            if (side > 0)
            {
                _rightRewinded = true;
            }
            else
            {
                _leftRewinded = true;
            }
        }
    }
}
