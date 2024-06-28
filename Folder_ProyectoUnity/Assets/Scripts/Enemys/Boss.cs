using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.AI;

public class Boss : HerenciaEnemy
{
    private Renderer enemyRenderer;
    public LibrarySounds BossSounds;
    public AudioClip dieSound;
    public VisualEffect VFX_die;
    private MaterialPropertyBlock mpb;
    private float dissolveAmount = 0f;
    private float dissolveSpeed = 1f;
    private float timer;

    [Header("Boss Movement")]
    public Transform objetivo;
    public float velocidad;
    public NavMeshAgent IA;
    private bool playerInArea = false;

    [Header("Boss Attack")]
    public BoxCollider handBoss;
    private bool isAttacking = false;
    private bool isTakingDamage = false;
    private bool isDead = false;
    private bool hasTaunted = false;

    public PlayerController player;
    public PlayerActions actionsPlayer;

    protected void Start()
    {
        damage = 10;
        maxHP = 40;
        currentHP = maxHP;
        pushingForce = 20;
        enemyRenderer = GetComponentInChildren<Renderer>();
        actionsPlayer = FindObjectOfType<PlayerActions>();
        mpb = new MaterialPropertyBlock();
        IA.speed = velocidad;
        DesactivarColliderBoss();
        StopAllBossActions();

    }

    private void OnEnable()
    {
        actionsPlayer.onPlayerEnterBossArea += HandlePlayerEnterBossArea;
    }

    private void OnDisable()
    {
        actionsPlayer.onPlayerEnterBossArea -= HandlePlayerEnterBossArea;
    }

    protected override void Update()
    {
        if (!isDead && actionsPlayer.inZone)
        {
            ResumeBossActions();
            HandleMovement();
            HandleSounds();
            TauntBoss();
        }
    }

    private void HandleMovement()
    {
        if (objetivo != null && !isDead)
        {
            IA.SetDestination(objetivo.position);
            if (!isAttacking && !isTakingDamage)
            {
                animator.SetBool("BossRun", true);
                IA.isStopped = false;
            }
        }
        else
        {
            animator.SetBool("BossRun", false);
            IA.isStopped = true;
        }
    }

    private void HandleSounds()
    {
        if (timer >= 10 && !player.isDead)
        {
            _audio.PlayOneShot(BossSounds.clipSounds[UnityEngine.Random.Range(5, 8)]);
            timer = 0;
        }

        timer += Time.deltaTime;
    }

    public void TakeDamage(int damage, Vector3 attackerPosition)
    {
        if (isDead) return;

        currentHP -= damage;
        if (currentHP <= 0)
        {
            Kill();
        }
        else
        {
            _audio.PlayOneShot(BossSounds.clipSounds[UnityEngine.Random.Range(0, 4)]);
            Vector3 direction = (transform.position - attackerPosition).normalized;
            rb.AddForce(direction * pushingForce, ForceMode.Impulse);
        }
    }

    public void Kill()
    {
        if (isDead) return;

        isDead = true;
        OnEnemyKilled?.Invoke();
        StopAllCoroutines();
        animator.SetBool("BossAttack", false);
        animator.SetBool("BossRun", false);
        StartCoroutine(DieBoss());
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isDead)
        {
            playerInArea = true;
            if (!isAttacking && !isTakingDamage)
            {
                StartCoroutine(BossAttack());
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInArea = false;
        }
    }

    public void TauntBoss()
    {
        if (player.isDead && !hasTaunted)
        {
            StartCoroutine(SoundClip());
        }
    }

    IEnumerator SoundClip()
    {
        animator.SetTrigger("Taunt");
        animator.SetBool("BossAttack", false);
        animator.SetBool("BossRun", false);
        IA.isStopped = true;
        hasTaunted = true;
        yield return new WaitForSeconds(1f);
        _audio.PlayOneShot(BossSounds.clipSounds[8]);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Arma") && !isDead)
        {
            Weapons weapon = other.gameObject.GetComponentInParent<Weapons>();
            if (weapon != null)
            {
                TakeDamage(weapon.Damage, weapon.transform.position);
            }
        }
    }

    IEnumerator BossAttack()
    {
        isAttacking = true;
        IA.isStopped = true;
        animator.SetBool("BossAttack", true);

        yield return new WaitForSeconds(2f);

        animator.SetBool("BossAttack", false);

        yield return new WaitForSeconds(0.1f);

        isAttacking = false;
        IA.isStopped = false;
    }

    IEnumerator DieBoss()
    {
        _audio.PlayOneShot(dieSound);
        animator.SetTrigger("BossDie");
        enemyCollider.enabled = false;
        yield return new WaitForSeconds(1f);
        VFX_die.Play();
        yield return new WaitForSeconds(3f);
        while (dissolveAmount < 1f)
        {
            dissolveAmount += Time.deltaTime * dissolveSpeed;
            SetDissolveAmount(dissolveAmount);
            yield return null;
        }

        Destroy(gameObject);
    }

    private void SetDissolveAmount(float amount)
    {
        mpb.SetFloat("_DissolveAmount", amount);
        enemyRenderer.SetPropertyBlock(mpb);
    }

    public void ActivarColliderBoss()
    {
        handBoss.enabled = true;
    }

    public void DesactivarColliderBoss()
    {
        handBoss.enabled = false;
    }

    private void StopAllBossActions()
    {
        animator.SetBool("BossRun", false);
        animator.SetBool("BossAttack", false);
        IA.isStopped = true;
    }

    private void ResumeBossActions()
    {
        IA.isStopped = false;
    }

    private void HandlePlayerEnterBossArea(bool entered)
    {
        if (entered)
        {
            Debug.Log("El jugador entró en el área del Boss");
        }
        else
        {
            Debug.Log("El jugador salió del área del Boss");
        }
    }
}
