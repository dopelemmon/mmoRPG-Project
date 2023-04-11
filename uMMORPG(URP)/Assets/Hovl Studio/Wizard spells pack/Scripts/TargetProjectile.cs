﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetProjectile : MonoBehaviour
{
    public EnemyAi[] enemy;
    public HS_MovementInput hS_MovementInput;
    // public EnemyAi enemyAi;
    public float speed = 15f;
    public GameObject hit;
    public GameObject flash;
    public GameObject[] Detached;
    public bool LocalRotation = false;
    private Transform target;
    private Vector3 targetOffset;

    [Space]
    [Header("PROJECTILE PATH")]
    private float randomUpAngle;
    private float randomSideAngle;
    public float sideAngle = 25;
    public float upAngle = 20;
    private float radius;

    void Start()
    {
        FlashEffect();
        newRandom();

        hS_MovementInput = GameObject.FindWithTag("Player").GetComponent<HS_MovementInput>();
        for(int i = 0; i < enemy.Length; i++)
        {
            
            enemy[i] = GameObject.FindWithTag("Player").GetComponent<HS_MovementInput>().screenTargets[i].GetComponent<EnemyAi>();
        }
    }

    void newRandom()
    {
        randomUpAngle = Random.Range(0, upAngle);
        randomSideAngle = Random.Range(-sideAngle, sideAngle);
    }

    //Link from movement controller
    //TARGET POSITION + TARGET OFFSET
    public void UpdateTarget(Transform targetPosition , Vector3 Offset)
    {
        target = targetPosition;
        targetOffset = Offset;
    }

    void Update()
    {
        if (target == null)
        {
            foreach (var detachedPrefab in Detached)
            {
                if (detachedPrefab != null)
                {
                    detachedPrefab.transform.parent = null;
                }
            }
            Destroy(gameObject);
            return;
        }

        Vector3 forward = ((target.position + targetOffset) - transform.position);
        Vector3 crossDirection = Vector3.Cross(forward, Vector3.up);
        Quaternion randomDeltaRotation = Quaternion.Euler(0, randomSideAngle, 0) * Quaternion.AngleAxis(randomUpAngle, crossDirection);
        Vector3 direction = randomDeltaRotation * ((target.position + targetOffset) - transform.position);

        float distanceThisFrame = Time.deltaTime * speed;

        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
        transform.rotation = Quaternion.LookRotation(direction);
    }

    void FlashEffect()
    {
        if (flash != null)
        {
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;
            var flashPs = flashInstance.GetComponent<ParticleSystem>();
            if (flashPs != null)
            {
                Destroy(flashInstance, flashPs.main.duration);
                
            }
            else
            {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(flashInstance, flashPsParts.main.duration);
            }
        }
    }

public void HitTarget()
{
    if (hit != null)
    {
        var hitRotation = transform.rotation;
        if (LocalRotation == true)
        {
            hitRotation = Quaternion.Euler(0, 0, 0);
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider collider in colliders)
        {
            EnemyAi enemy = collider.GetComponent<EnemyAi>();
            if (enemy != null && hS_MovementInput.activeTarger)
            {
                // Instantiate hit effect
                var hitInstance = Instantiate(hit, enemy.transform.position + targetOffset, hitRotation);
                var hitPs = hitInstance.GetComponent<ParticleSystem>();
                if (hitPs != null)
                {
                    Destroy(hitInstance, hitPs.main.duration);
                    enemy.takeDamage(5);
                }
                else
                {
                    var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy(hitInstance, hitPsParts.main.duration);
                }
            }
        }
    }

    foreach (var detachedPrefab in Detached)
    {
        if (detachedPrefab != null)
        {
            detachedPrefab.transform.parent = null;
        }
    }
    Destroy(gameObject);
}




}
