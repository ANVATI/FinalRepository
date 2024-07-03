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

    protected SimpleList<GameObject> patrolRoute = new SimpleList<GameObject>();

    protected int currentPatrolIndex = 0;
    protected GameObject currentTargetNode;

    protected float dissolveAmount = 0f;
    protected float dissolveSpeed = 1f;
    protected float timer;
    protected bool playerInCollision = false;
    protected bool isDead = false;
    protected Renderer enemyRenderer;
    protected bool isPaused = false;
    protected bool isChasingPlayer = false;
    protected bool isAttacking = false;
    protected bool isTakingDamage = false;
    protected bool isDying = false;

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
