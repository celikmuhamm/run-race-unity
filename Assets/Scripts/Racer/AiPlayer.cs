using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPlayer : MonoBehaviour
{
    public float RayPulseTimer = 0.3f;    // Time intervals between casted rays. 
    public int rayCount = 3;
    public Vector3 rayAngles = new Vector3(0,15.0f,0 );
    public float rayDistance = 3.0f;
    public Vector3 rayOffset = new Vector3(0,1,0);
    public Racer racer;
    private Ray[] _rayArray;
    private bool[] _isHitARray;
    private DelayFunction _delayer = new DelayFunction();
    
    private void OnDrawGizmos()
    {
        if (_rayArray!=null && _rayArray.Length> 0)
        {
            for (int i = 0; i < rayCount; i++)
            {
                if (_isHitARray[i])
                {
                    Gizmos.color = Color.red;;
                    Gizmos.DrawLine( this.transform.position + rayOffset, _rayArray[i].GetPoint(rayDistance));
                }
                else
                {
                    Gizmos.color = Color.white;;
                    Gizmos.DrawLine(  this.transform.position + rayOffset, _rayArray[i].GetPoint(rayDistance));
                }
            }
        }
        
    }

    private void Start()
    {
        _rayArray = new Ray[rayCount];
        _isHitARray = new bool[rayCount];
        _delayer.Delay(RayCast, RayPulseTimer);
    }

    private void FixedUpdate()
    {
        _delayer.Update();
        
    }

    private void RayCast()
    {
        bool isHit = false;
        for (int i = 0; i < rayCount; i++)
        {
            int currentDegreeMultiplier = i - rayCount / 2;
            Ray ray = new Ray(transform.position + rayOffset, Quaternion.Euler(currentDegreeMultiplier*rayAngles)*transform.forward);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, rayDistance))
            {
                if (hitInfo.collider.gameObject.CompareTag("InGameObject") ||hitInfo.collider.gameObject.CompareTag("aiStop") )
                {
                    _rayArray[i] = ray;
                    _isHitARray[i] = true;
                    racer.Stop();
                    isHit = true;
                }else
                {
                    _rayArray[i] = ray;
                    _isHitARray[i] = false;
                }
               
            }
            else
            {
                _rayArray[i] = ray;
                _isHitARray[i] = false;
            }
        }

        if (!isHit)
        {
            racer.Run();
        }
        _delayer.Delay(RayCast, RayPulseTimer);
    }

    private void SetArrayElement()
    {
        
    }
}
