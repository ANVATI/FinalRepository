using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class HerenciaEnemy : MonoBehaviour
{
    [Header("Componentes Heredados")]
    public static Action OnEnemyKilled;
    protected Animator animator;
    protected Rigidbody rb;
    protected Collider enemyCollider;
    protected AudioSource _audio;
    public PlayerActions playerAction;
    public PlayerController player;
    protected MaterialPropertyBlock mpb;
    protected NavMeshAgent IA;

    [Header("Atributos Heredados")]
    protected int currentHP; 
    protected int maxHP;
    protected int speed;
    public int damage;
    protected int pushingForce;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        enemyCollider = GetComponent<Collider>();
        _audio = GetComponent<AudioSource>();
        IA = GetComponent<NavMeshAgent>();
        playerAction = FindObjectOfType<PlayerActions>();
        player = FindAnyObjectByType<PlayerController>();
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
