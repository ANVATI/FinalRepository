using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Goblin : HerenciaEnemy
{
    public Graph graph;
    public GameObject[] patrolRouteArray;
    protected SimpleList<GameObject> patrolRoute = new SimpleList<GameObject>();

    protected int currentPatrolIndex = 0;
    protected GameObject currentTargetNode;

    public float detectionRadius = 10f;
    public Transform playerFollow;

    public float chaseSpeed = 6f;

    protected bool isPaused = false;
    protected bool isChasingPlayer = false;
    protected bool isAttacking = false;
    protected bool isTakingDamage = false;
    protected bool isDying = false;

    protected Renderer enemyRenderer;
    public LibrarySounds goblinSounds;
    public GameObject Eyes;
    protected float dissolveAmount = 0f;
    protected float dissolveSpeed = 1f;
    protected float timer;
    protected bool playerInCollision = false;
    protected bool isDead = false;

    private NavMeshAgent navMeshAgent;

    protected override void Awake()
    {
        base.Awake();
        speed = 2;
        enemyRenderer = GetComponentInChildren<Renderer>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
        navMeshAgent.enabled = false;
        maxHP = 50;
        currentHP = maxHP;
        pushingForce = 10;
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

        if (isDead) return;

        if (isDying)
            return;

        if (isTakingDamage)
            return;

        if (isChasingPlayer && !isAttacking)
        {
            animator.SetBool("GoblinWalk", false);
            animator.SetBool("GoblinRun", true);
            navMeshAgent.speed = chaseSpeed;
            if (navMeshAgent.enabled)
            {
                navMeshAgent.SetDestination(playerFollow.position);
            }
            LookAtTarget(playerFollow.position);
        }
        else if (!isAttacking)
        {
            if (!isPaused && currentTargetNode != null)
            {
                animator.SetBool("GoblinRun", false);
                animator.SetBool("GoblinWalk", true);
                navMeshAgent.speed = speed;
                if (navMeshAgent.enabled)
                {
                    navMeshAgent.SetDestination(currentTargetNode.transform.position);
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
            navMeshAgent.enabled = true;
            navMeshAgent.speed = chaseSpeed;
            navMeshAgent.SetDestination(playerFollow.position);
        }
    }

    protected void StopChasingPlayer()
    {
        if (isChasingPlayer && !isAttacking)
        {
            animator.SetBool("GoblinRun", false);
            animator.SetBool("GoblinWalk", true);
            isChasingPlayer = false;
            navMeshAgent.enabled = false;
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

            if (isAttacking)
            {
                StopCoroutine(AttackCoroutine());
                isAttacking = false;
                animator.SetBool("GoblinAttack", false);
            }

            StartCoroutine(TakeDamageCoroutine());
        }
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
        if (collision.gameObject.tag == "Player")
        {
            if (!isAttacking && !playerInCollision && !isTakingDamage && !isDying)
            {
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
        }
    }

    IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        animator.SetBool("GoblinWalk", false);
        animator.SetBool("GoblinRun", false);
        animator.SetBool("GoblinAttack", true);
        navMeshAgent.isStopped = true;

        yield return new WaitForSeconds(1.5f);

        if (!isDying)
        {
            isAttacking = false;
            animator.SetBool("GoblinWalk", true);
            animator.SetBool("GoblinAttack", false);
            navMeshAgent.isStopped = false;
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Arma")
        {
            Weapons weapon = other.gameObject.GetComponentInParent<Weapons>();
            if (weapon != null)
            {
                TakeDamage(weapon.Damage, weapon.transform.position);
            }
        }
    }
}
