using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class GroundTrap : InGameObject
{
    public float maxYPosition = 1.8f;
    public float minYPosition = 0.3f;
    public float riseSpeed = 1.0f;
    public float fallSpeed = 0.5f;
    public float totalTime = 3f;
    public float expForceMagnitude = 500;
    public float expForceRange = 50;
    public int risingTrapIndex = -1;
    [SerializeField] private Ease movementEase;
    [SerializeField] private Transform[] trapObjects;
    private bool _movingForTheFirstTime = true;
    private DelayFunction variationDelay;

    private void Start()
    {
        variationDelay = new DelayFunction();
        variationDelay.Delay(StartMove, Random.Range(0, variationTimer));
    }

    private void StartMove()
    {
        StartCoroutine(Move());
    }

    private void FixedUpdate()
    {
        variationDelay.Update();
    }

    public override void makeAction(Racer racer)
    {
        racer.Die();
        if (racer.hitCollider.transform.GetSiblingIndex() == risingTrapIndex)
        {
            Vector3 forcePosition = trapObjects[risingTrapIndex].position;
            Collider[] touchedColliders = Physics.OverlapSphere(forcePosition, expForceRange);
            foreach (var collider in touchedColliders)
            {
                Rigidbody rigidbody = collider.GetComponent<Rigidbody>();
                if (rigidbody != null)
                {
                    rigidbody.AddExplosionForce(expForceMagnitude, forcePosition, expForceRange);
                }
            }
        }
    }


    private IEnumerator Move()
    {
        while (true)
        {
            for (int i = 0; i < trapObjects.Length; i++)
            {
                Transform trap = trapObjects[i];
                risingTrapIndex = i;
                if (i == 0 && _movingForTheFirstTime)
                {
                    _movingForTheFirstTime = false;
                    yield return new WaitForSeconds(totalTime / levelMultiplier);
                }
                else if (i == 0)
                {
                    yield return new WaitForSeconds(totalTime / levelMultiplier +
                                                    totalTime / fallSpeed / levelMultiplier);
                }

                yield return trap.DOLocalMoveY(maxYPosition, totalTime / riseSpeed / levelMultiplier, false)
                    .SetEase(movementEase)
                    .OnComplete(() => trap.DOLocalMoveY(minYPosition, totalTime / fallSpeed / levelMultiplier, false))
                    .WaitForCompletion();
            }
        }
    }
}