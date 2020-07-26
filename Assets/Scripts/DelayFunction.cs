using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayFunction 
{
    private float _timer = 0; 
    private Action _function;
    private bool _isDelayed = false;
    public void Delay(Action function, float time)
    {
        _function = function;
        _timer = time;
        _isDelayed = true;
    }
    public void Update()
    {
        if (_isDelayed)
        {
            CountDown(Time.deltaTime);
            if (_timer <= 0)
            {
                _isDelayed = false;
                _function();
            }
        }
       
    }

    public void CountDown(float interval)
    {
        _timer -= interval;
    }
}
