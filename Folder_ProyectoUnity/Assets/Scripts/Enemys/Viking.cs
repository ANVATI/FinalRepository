using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class Viking : HerenciaEnemy
{
    [Header("Sistema de Grafos")]
    public Graph graph;
    public GameObject[] patrolRouteArray;

    [Header("Detección y Ataque")]
    public float detectionRadius;
    public Transform playerFollow;
    public float chaseSpeed;
    public BoxCollider mazo;

    [Header("Efectos de Sonido")]
    public LibrarySounds vikingSounds;

    protected override void Awake()
    {
        mazo.enabled = false;
        base.Awake();
        speed = 2;
        IA = GetComponent<NavMeshAgent>();
        IA.speed = speed;
        maxHP = 60;
        currentHP = maxHP;
        pushingForce = 20;
        InitializePatrolRoute();
        InitializeEnemy();
    }

    protected void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 8)
        {
            _audio.PlayOneShot(vikingSounds.clipSounds[6]);
            timer = 0;
        }

        if (isDead) return;

        if (isDying)
            return;

        if (isTakingDamage)
            return;

        if (isChasingPlayer && !isAttacking)
        {
            animator.SetBool("VikingWalk", false);
            animator.SetBool("VikingRun", true);
            IA.speed = chaseSpeed;
            if (IA.enabled && IA.isOnNavMesh)
            {
                IA.SetDestination(playerFollow.position);
            }
            LookAtTarget(playerFollow.position);
        }
        else if (!isAttacking)
        {
            if (!isPaused && currentTargetNode != null)
            {
                animator.SetBool("VikingRun", false);
                animator.SetBool("VikingWalk", true);
                IA.speed = speed;
                if (IA.enabled && IA.isOnNavMesh)
                {
                    IA.SetDestination(currentTargetNode.transform.position);
                }
                LookAtTarget(currentTargetNode.transform.position);

                if (Vector3.Distance(transform.position, currentTargetNode.transform.position) < 0.1f)
                {
                    StartCoroutine(PauseAndRechargeEnergy());
                }
            }
        }

        if (Vector3.Distance(transform.position, playerFollow.position) <= detectionRadius && !isAttacking)
        {
            StartChasingPlayer();
        }
        else if (isChasingPlayer && !isAttacking)
        {
            StopChasingPlayer();
        }
    }

    protected void InitializePatrolRoute()
    {
        patrolRoute.Clear();
        for (int i = 0; i < patrolRouteArray.Length; i++)
        {
            patrolRoute.Add(patrolRouteArray[i]);
        }
    }

    protected void InitializeEnemy()
    {
        if (patrolRoute.Length > 0)
        {
            currentTargetNode = patrolRoute.Get(currentPatrolIndex);
            if (IA.enabled && IA.isOnNavMesh)
            {
                IA.SetDestination(currentTargetNode.transform.position);
            }
        }
    }

    protected void UpdatePatrolRoute()
    {
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolRoute.Length;
        currentTargetNode = patrolRoute.Get(currentPatrolIndex);
        if (IA.enabled && IA.isOnNavMesh)
        {
            IA.SetDestination(currentTargetNode.transform.position);
        }
    }

    IEnumerator PauseAndRechargeEnergy()
    {
        animator.SetBool("VikingWalk", false);
        isPaused = true;
        yield return new WaitForSeconds(2.5f);
        isPaused = false;
        animator.SetBool("VikingWalk", true);
        UpdatePatrolRoute();
    }

    protected void StartChasingPlayer()
    {
        if (!isChasingPlayer && !isAttacking)
        {
            animator.SetBool("VikingRun", true);
            isChasingPlayer = true;
            isPaused = false;
            IA.enabled = true;
            IA.speed = chaseSpeed;
            IA.SetDestination(playerFollow.position);
        }
    }

    protected void StopChasingPlayer()
    {
        if (isChasingPlayer && !isAttacking)
        {
            animator.SetBool("VikingRun", false);
            animator.SetBool("VikingWalk", true);
            isChasingPlayer = false;
            IA.enabled = false;
            InitializeEnemy();
        }
    }

    protected void LookAtTarget(Vector3 targetPosition)
    {
        if (isDead) return;

        Vector3 lookDirection = targetPosition - transform.position;
        lookDirection.y = 0;
        if (lookDirection != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
        }
    }

    public void TakeDamage(int damage, Vector3 attackerPosition)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            Kill();
        }
        else
        {
            _audio.PlayOneShot(vikingSounds.clipSounds[Random.Range(0, 4)]);
            Vector3 direction = (transform.position - attackerPosition).normalized;
            rb.AddForce(direction * pushingForce, ForceMode.Impulse);

            if (currentHP <= 20)
            {
                ShrinkViking();
            }
            if (isAttacking)
            {
                StopCoroutine(AttackCoroutine());
                isAttacking = false;
                animator.SetBool("VikingAttack", false);
            }

            StartCoroutine(TakeDamageCoroutine());
        }
    }

    private void ShrinkViking()
    {
        transform.DOScale(new Vector3(1.03f, 1.03f, 1.03f), 1f).SetEase(Ease.InOutQuad);
        damage = damage + 5;
    }

    public void Kill()
    {
        OnEnemyKilled?.Invoke();
        _audio.PlayOneShot(vikingSounds.clipSounds[5]);
        isDead = true;
        StartCoroutine(DieViking());
    }

    IEnumerator DieViking()
    {
        if (isDying)
            yield break;

        enemyCollider.enabled = false;
        rb.constraints |= RigidbodyConstraints.FreezePositionX;
        rb.constraints |= RigidbodyConstraints.FreezePositionZ;
        isDying = true;
        animator.SetBool("VikingWalk", false);
        animator.SetBool("VikingRun", false);
        animator.SetBool("VikingAttack", false);
        animator.SetBool("VikingHit", false);
        animator.SetTrigger("VikingDie");

        yield return new WaitForSeconds(5f);

        Destroy(gameObject);
    }

    IEnumerator TakeDamageCoroutine()
    {
        isTakingDamage = true;
        animator.SetBool("VikingWalk", false);
        animator.SetBool("VikingRun", false);
        animator.SetBool("VikingAttack", false);
        animator.SetBool("VikingHit", true);

        yield return new WaitForSeconds(0.8f);

        isTakingDamage = false;
        animator.SetBool("VikingHit", false);
    }

    protected void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!isAttacking && !playerInCollision && !isTakingDamage && !isDying)
            {
                rb.constraints |= RigidbodyConstraints.FreezePositionX;
                rb.constraints |= RigidbodyConstraints.FreezePositionZ;
                playerInCollision = true;
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    protected void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInCollision = false;
            rb.constraints = rb.constraints & ~RigidbodyConstraints.FreezePositionX;
            rb.constraints = rb.constraints & ~RigidbodyConstraints.FreezePositionZ;
        }
    }

    IEnumerator AttackCoroutine()
    {
        while (playerInCollision)
        {
            isAttacking = true;
            animator.SetBool("VikingWalk", false);
            animator.SetBool("VikingRun", false);
            animator.SetBool("VikingAttack", true);
            IA.isStopped = true;
            yield return new WaitForSeconds(1.5f);

            if (!isDying)
            {
                isAttacking = false;
                animator.SetBool("VikingWalk", true);
                animator.SetBool("VikingAttack", false);
                IA.isStopped = false;
            }
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sword"))
        {
            Weapons weapon = other.gameObject.GetComponentInParent<Weapons>();
            if (weapon != null)
            {
                TakeDamage(weapon.Damage + 10, weapon.transform.position);
            }
        }
        else if (other.gameObject.CompareTag("Axe"))
        {
            Weapons weapon = other.gameObject.GetComponentInParent<Weapons>();
            if (weapon != null)
            {
                TakeDamage(weapon.Damage, weapon.transform.position);
            }
        }
        else if (other.gameObject.CompareTag("Axe1"))
        {
            Weapons weapon = other.gameObject.GetComponentInParent<Weapons>();
            if (weapon != null)
            {
                TakeDamage(weapon.Damage, weapon.transform.position);
            }
        }
    }

    public void ActivarColliderViking()
    {
        mazo.enabled = true;
    }
    public void DesactivarColliderViking()
    {
        mazo.enabled = false;
    }
}
