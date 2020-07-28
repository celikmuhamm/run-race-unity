using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : InGameObject
{
    public float outXPosition = 1.8f;
    public float speed = 1.0f;
    public float totalTime = 3f;
    public float fallTime = 1.5f;
    [SerializeField] private Ease movementEase;
    [SerializeField] private Transform movingTransform;
    private bool state = true;
    private Tweener _animation;
    private DelayFunction _animationDelayCreator;
    private DelayFunction _fallDelayCreator;
    private void Awake()
    {
        _animationDelayCreator = new DelayFunction();
        _fallDelayCreator = new DelayFunction();
        _animation = movingTransform
            .DOLocalMoveX(outXPosition, totalTime / speed / levelMultiplier, false)
            .SetEase(movementEase)
            .SetAutoKill(false).OnStepComplete(()=> Completed());
    }

    private void FixedUpdate()
    {
        _animationDelayCreator.Update();
        _fallDelayCreator.Update();
    }

   

    private void Completed()
    {
        if (state)
        {
            _animationDelayCreator.Delay(_animation.PlayBackwards,totalTime/levelMultiplier/2);
        }
        else
        {
            _animationDelayCreator.Delay(_animation.PlayForward,totalTime/levelMultiplier);
            
        }

        state = !state;
    }

    public override void makeAction(Racer racer)
    {
       racer.GetComponent<Animator>().SetTrigger("Fall");
       racer.GetComponent<Animator>().applyRootMotion = false;
       _fallDelayCreator.Delay(racer.Die,fallTime);
    }
}
