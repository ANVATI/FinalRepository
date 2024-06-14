using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

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

    protected void Start()
    {
        maxHP = 10;
        currentHP = maxHP;
        pushingForce = 20;
        enemyRenderer = GetComponentInChildren<Renderer>();
        mpb = new MaterialPropertyBlock();
    }
    protected override void Update()
    {
        if (timer >= 10)
        {
            _audio.PlayOneShot(BossSounds.clipSounds[Random.Range(5, 8)]);
            timer = 0;
        }

        timer = timer + Time.deltaTime;
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
            _audio.PlayOneShot(BossSounds.clipSounds[Random.Range(0,4)]);
            Vector3 direction = (transform.position - attackerPosition).normalized;
            rb.AddForce(direction * pushingForce, ForceMode.Impulse);
        }
    }

    public void Kill()
    {
        OnEnemyKilled?.Invoke();
        StopCoroutine(IntHint());
        animator.SetBool("BossHit", false);
        StartCoroutine(DieBoss());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Arma")
        {
            Weapons weapon = other.gameObject.GetComponentInParent<Weapons>();
            if (weapon != null)
            {
                StartCoroutine(IntHint());
                TakeDamage(weapon.Damage, weapon.transform.position);
            }
        }
    }

    IEnumerator IntHint()
    {
        animator.SetBool("BossHit", true);
        yield return new WaitForSeconds(0.8f);
        animator.SetBool("BossHit", false);
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
}
