using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using DG.Tweening;

public class Goblin : HerenciaEnemy
{
    [Header("Sistema de Grafos")]
    public Graph graph;
    public GameObject[] patrolRouteArray;

    [Header("Detecci�n y Ataque")]
    public BoxCollider goblinAxe;
    public float detectionRadius;
    public Transform playerFollow;
    public float chaseSpeed;
    public bool IncaveArea;

    public LibrarySounds goblinSounds;
    public GameObject Eyes;
    public GameObject Axe;

    protected override void Awake()
    {
        goblinAxe.enabled = false;
        base.Awake();
        speed = 2;
        enemyRenderer = GetComponentInChildren<Renderer>();
        IA = GetComponent<NavMeshAgent>();
        IA.speed = speed;
        IA.enabled = false;
        maxHP = 35;
        damage = 5;
        currentHP = maxHP;
        pushingForce = 20;
        enemyRenderer = GetComponentInChildren<Renderer>();
        mpb = new MaterialPropertyBlock();
        InitializePatrolRoute();
        InitializeEnemy();
    }

    protected void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 8)
        {
            _audio.PlayOneShot(goblinSounds.clipSounds[3]);
            timer = 0;
        }

        if (isDead) 
            return;

        if (isDying)
            return;

        if (isTakingDamage)
            return;

        if (isChasingPlayer && !isAttacking)
        {
            animator.SetBool("GoblinWalk", false);
            animator.SetBool("GoblinRun", true);
            IA.speed = chaseSpeed;
            if (IA.enabled)
            {
                IA.SetDestination(playerFollow.position);
            }
            LookAtTarget(playerFollow.position);
        }
        else if (!isAttacking)
        {
            if (!isPaused && currentTargetNode != null)
            {
                animator.SetBool("GoblinRun", false);
                animator.SetBool("GoblinWalk", true);
                IA.speed = speed;
                if (IA.enabled)
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
        }
    }

    protected void UpdatePatrolRoute()
    {
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolRoute.Length;
        currentTargetNode = patrolRoute.Get(currentPatrolIndex);
    }

    IEnumerator PauseAndRechargeEnergy()
    {
        animator.SetBool("GoblinWalk", false);
        isPaused = true;
        yield return new WaitForSeconds(2.5f);
        isPaused = false;
        animator.SetBool("GoblinWalk", true);
        UpdatePatrolRoute();
    }

    protected void StartChasingPlayer()
    {
        if (!isChasingPlayer && !isAttacking)
        {
            animator.SetBool("GoblinRun", true);
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
            animator.SetBool("GoblinRun", false);
            animator.SetBool("GoblinWalk", true);
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
            _audio.PlayOneShot(goblinSounds.clipSounds[0]);
            Vector3 direction = (transform.position - attackerPosition).normalized;
            rb.AddForce(direction * pushingForce, ForceMode.Impulse);

            if (currentHP <= 10 && IncaveArea)
            {
                ShrinkGoblin();
            }

            if (isAttacking)
            {
                StopCoroutine(AttackCoroutine());
                isAttacking = false;
                animator.SetBool("GoblinAttack", false);
            }

            StartCoroutine(TakeDamageCoroutine());
        }
    }

    private void ShrinkGoblin()
    {
        transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 1f).SetEase(Ease.InOutQuad);
        speed = speed + 2;
    }

    public void Kill()
    {
        OnEnemyKilled?.Invoke();
        _audio.PlayOneShot(goblinSounds.clipSounds[1]);
        isDead = true;
        StartCoroutine(DieGoblin());
    }

    IEnumerator TakeDamageCoroutine()
    {
        isTakingDamage = true;
        animator.SetBool("GoblinWalk", false);
        animator.SetBool("GoblinRun", false);
        animator.SetBool("GoblinAttack", false);
        animator.SetBool("GoblinHit", true);

        yield return new WaitForSeconds(0.8f);

        isTakingDamage = false;
        animator.SetBool("GoblinHit", false);
    }

    IEnumerator DieGoblin()
    {
        if (isDying)
            yield break;

        isDying = true;
        animator.SetBool("GoblinWalk", false);
        animator.SetBool("GoblinRun", false);
        animator.SetBool("GoblinAttack", false);
        animator.SetBool("GoblinHit", false);
        animator.SetTrigger("GoblinDie");

        enemyCollider.enabled = false;
        Destroy(Eyes.gameObject);
        Destroy(Axe.gameObject);
        yield return new WaitForSeconds(1.5f);
        _audio.PlayOneShot(goblinSounds.clipSounds[2]);
        yield return new WaitForSeconds(0.75f);
        while (dissolveAmount < 1f)
        {
            dissolveAmount += Time.deltaTime * dissolveSpeed;
            SetDissolveAmount(dissolveAmount);
            yield return null;
        }

        Destroy(gameObject);
    }

    protected void SetDissolveAmount(float amount)
    {
        mpb.SetFloat("_DissolveAmount", amount);
        enemyRenderer.SetPropertyBlock(mpb);
    }

    protected void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isAttacking && !isTakingDamage && !isDying)
            {
                playerInCollision = true;
                StartCoroutine(AttackCoroutine());
            }
        }
        else if (collision.gameObject.CompareTag("CaveFloor"))
        {
            IncaveArea = true;
        }
    }

    protected void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInCollision = false;
        }
    }

    IEnumerator AttackCoroutine()
    {
        while (playerInCollision)
        {
            isAttacking = true;
            animator.SetBool("GoblinWalk", false);
            animator.SetBool("GoblinRun", false);
            animator.SetBool("GoblinAttack", true);
            IA.isStopped = true;
            yield return new WaitForSeconds(1.5f);

            if (!isDying)
            {
                isAttacking = false;
                animator.SetBool("GoblinWalk", true);
                animator.SetBool("GoblinAttack", false);
                IA.isStopped = false;
            }
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Axe"))
        {
            Weapons weapon = other.gameObject.GetComponentInParent<Weapons>();
            if (weapon != null)
            {
                TakeDamage(weapon.Damage + 10, weapon.transform.position);
            }
        }
        else if (other.gameObject.CompareTag("Sword"))
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

    public void ActivarColliderGoblin()
    {
        goblinAxe.enabled = true;
    }

    public void DesactivarColliderGoblin()
    {
        goblinAxe.enabled = false;
    }
}
