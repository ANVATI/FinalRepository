using System.Collections;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.AI;

public class Boss : HerenciaEnemy
{
    private Renderer enemyRenderer;
    public LibrarySounds BossSounds;
    public VisualEffect VFX_die;
    private float dissolveAmount = 0f;
    private float dissolveSpeed = 1f;
    private int totalDamageTaken = 0;
    private float timer;

    [Header("Boss Movement")]
    public Transform objetivo;
    private bool isSpecialAttacking = false;

    [Header("Boss Attack")]
    public BoxCollider handBoss;
    private bool isAttacking = false;
    private bool isTakingDamage = false;
    private bool isDead = false;
    private bool hasTaunted = false;

    [Header("Class References")]
    public AudioSource footSteps;

    [Header("SpecialAttack")]
    public GameObject Area;
    public GameObject colisionAttack;

    [Header("ActivateSound")]
    public GameObject musicCave;
    protected void Start()
    {
        maxHP = 30;
        currentHP = maxHP;
        pushingForce = 20;
        speed = 5;
        enemyRenderer = GetComponentInChildren<Renderer>();
        mpb = new MaterialPropertyBlock();
        IA.speed = speed;
        DesactivarColliderBoss();
        StopAllBossActions();

        footSteps = GetComponents<AudioSource>()[1];
    }

    private void OnEnable()
    {
        playerAction.onPlayerEnterBossArea += ResumeBossActions;
        playerAction.onPlayerEnterBossArea += HandleSounds;
        playerAction.onPlayerEnterBossArea += TauntBoss;
    }

    private void OnDisable()
    {
        playerAction.onPlayerEnterBossArea -= ResumeBossActions;
        playerAction.onPlayerEnterBossArea -= HandleSounds;
        playerAction.onPlayerEnterBossArea -= TauntBoss;
    }

    private void Update()
    {
        if (!isDead && playerAction.inZone)
        {
            musicCave.SetActive(true);
            ResumeBossActions();
        }

        if (Time.timeScale <= 0 && footSteps.isPlaying)
        {
            footSteps.Stop();
        }
    }

    private void HandleMovement()
    {
        if (objetivo != null && !isDead && !isSpecialAttacking)
        {
            if (!isAttacking && !isTakingDamage)
            {
                IA.SetDestination(objetivo.position);
                animator.SetBool("BossRun", true);
                IA.isStopped = false;

                if (!footSteps.isPlaying && Time.timeScale > 0 && !player.isDead && !isSpecialAttacking)
                {
                    footSteps.Play();
                }
            }
            else
            {
                animator.SetBool("BossRun", false);
                IA.isStopped = true;

                if (footSteps.isPlaying)
                {
                    footSteps.Stop();
                }
            }
        }
    }

    private void HandleSounds()
    {
        timer = timer + Time.deltaTime;
        if (timer >= 8 && !player.isDead)
        {
            _audio.PlayOneShot(BossSounds.clipSounds[UnityEngine.Random.Range(5, 8)]);
            timer = 0;
        }
    }

    public void TakeDamage(int damage, Vector3 attackerPosition)
    {
        if (isDead) return;

        currentHP -= damage;
        totalDamageTaken += damage;

        int damageThreshold = 10;
        if (totalDamageTaken >= damageThreshold && !isSpecialAttacking)
        {
            StartCoroutine(SpecialAttackZone());
            totalDamageTaken = 0;
        }

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
        if (footSteps.isPlaying)
        {
            footSteps.Stop();
        }
        StartCoroutine(DieBoss());
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isDead)
        {
            if (!isAttacking && !isTakingDamage && !player.isDead)
            {
                StartCoroutine(BossAttack());
            }
        }
    }

    public void TauntBoss()
    {
        if (player.isDead && !hasTaunted)
        {
            footSteps.Stop();
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

        if (!isSpecialAttacking)
        {
            animator.SetBool("BossAttack", true);
            yield return new WaitForSeconds(1f);
            _audio.PlayOneShot(BossSounds.clipSounds[9]);
            yield return new WaitForSeconds(2f);
            animator.SetBool("BossAttack", false);
        }

        yield return new WaitForSeconds(0.1f);

        isAttacking = false;

        if (!isDead && !isSpecialAttacking)
        {
            IA.isStopped = false;
            HandleMovement();
        }

        if (totalDamageTaken >= 10 && !isSpecialAttacking)
        {
            StartCoroutine(SpecialAttackZone());
            totalDamageTaken = 0;
        }
    }

    IEnumerator DieBoss()
    {
        _audio.PlayOneShot(BossSounds.clipSounds[12]);
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
        if (footSteps.isPlaying)
        {
            footSteps.Stop();
        }
    }

    private void ResumeBossActions()
    {
        IA.isStopped = false;
        HandleMovement();
        HandleSounds();
        TauntBoss();
    }

    IEnumerator SpecialAttackZone()
    {
        isSpecialAttacking = true;
        IA.isStopped = true;
        animator.SetBool("BossRun", false);
        animator.SetBool("BossAttack", false);
        Area.SetActive(true);
        _audio.PlayOneShot(BossSounds.clipSounds[11]);

        yield return new WaitForSeconds(1f);

        animator.SetBool("SpecialAttack", true);

        yield return new WaitForSeconds(2f);
        colisionAttack.SetActive(true);
        _audio.PlayOneShot(BossSounds.clipSounds[10]);

        yield return new WaitForSeconds(1f);

        animator.SetBool("SpecialAttack", false);
        Area.SetActive(false);
        colisionAttack.SetActive(false);

        isSpecialAttacking = false;
        if (!isDead && !player.isDead)
        {
            IA.isStopped = false;
            HandleMovement();
        }
    }
}