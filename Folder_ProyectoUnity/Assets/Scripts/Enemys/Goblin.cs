using System.Collections;
using UnityEngine;

public class Goblin : HerenciaEnemy
{
    private Renderer enemyRenderer;
    public LibrarySounds goblinSounds;
    public GameObject Eyes;
    private MaterialPropertyBlock mpb;
    private float dissolveAmount = 0f;
    private float dissolveSpeed = 1f;
    private float timer;


    protected override void Update()
    {
        timer = timer + Time.deltaTime;

        if (timer >= 8)
        {
            _audio.PlayOneShot(goblinSounds.clipSounds[3]);
            timer = 0;
        }
    }

    private void Start()
    {
        maxHP = 1;
        currentHP = maxHP;
        pushingForce = 10;
        enemyRenderer = GetComponentInChildren<Renderer>();
        mpb = new MaterialPropertyBlock();
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
        }
    }

    public void Kill()
    {
        OnEnemyKilled?.Invoke();
        _audio.PlayOneShot(goblinSounds.clipSounds[1]);
        StopCoroutine(IntHint());
        animator.SetBool("GoblinHit", false);
        StartCoroutine(DieGoblin());
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
        animator.SetBool("GoblinHit", true);
        yield return new WaitForSeconds(0.8f);
        animator.SetBool("GoblinHit", false);
    }

    IEnumerator DieGoblin()
    {
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

    private void SetDissolveAmount(float amount)
    {
        mpb.SetFloat("_DissolveAmount", amount);
        enemyRenderer.SetPropertyBlock(mpb);
    }
}