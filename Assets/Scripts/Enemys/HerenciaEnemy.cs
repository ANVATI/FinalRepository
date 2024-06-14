using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HerenciaEnemy : MonoBehaviour
{
    public static Action OnEnemyKilled;
    protected Animator animator;
    protected Rigidbody rb;
    protected Collider enemyCollider;
    public Transform playerTransform;
    protected AudioSource _audio;
    protected int currentHP; 
    protected int maxHP;
    protected int pushingForce;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        enemyCollider = GetComponent<Collider>();
        _audio = GetComponent<AudioSource>();    
    }

    protected virtual void Update()
    {
        /*
        if (isAlive)
        {
            transform.LookAt(playerTransform);
        }
        else if (!isAlive)
        {
            Debug.Log("El enemigo dejó de mirar al jugador");
            transform.LookAt(transform.position);
        }
        */
    }

    public int GetCurrentHP()
    {
        return currentHP;
    }

    public int GetMaxHP()
    {
        return maxHP;
    }
}