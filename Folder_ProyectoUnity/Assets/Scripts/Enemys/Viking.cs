using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viking : HerenciaEnemy
{
    //private Renderer enemyRenderer;
    public LibrarySounds vikingSounds;
    //private MaterialPropertyBlock mpb;
    //private float dissolveAmount = 0f;
    //private float dissolveSpeed = 1f;
    private float timer;


    protected override void Update()
    {
        timer = timer + Time.deltaTime;

        if (timer >= 8)
        {
            _audio.PlayOneShot(vikingSounds.clipSounds[6]);
            timer = 0;
        }
    }

    private void Start()
    {
        maxHP = 10;
        currentHP = maxHP;
        pushingForce = 10;
        //enemyRenderer = GetComponentInChildren<Renderer>();
        //mpb = new MaterialPropertyBlock();
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
            _audio.PlayOneShot(vikingSounds.clipSounds[Random.Range(0,5)]);
            Vector3 direction = (transform.position - attackerPosition).normalized;
            rb.AddForce(direction * pushingForce, ForceMode.Impulse);
        }
    }

    public void Kill()
    {
        OnEnemyKilled?.Invoke();
        _audio.PlayOneShot(vikingSounds.clipSounds[5]);
        StopCoroutine(IntHint());
        animator.SetBool("VikingHit", false);
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
        animator.SetBool("VikingHit", true);
        yield return new WaitForSeconds(0.8f);
        animator.SetBool("VikingHit", false);
    }

    IEnumerator DieGoblin()
    {
        animator.SetTrigger("VikingDie");
        enemyCollider.enabled = false;
        rb.constraints |= RigidbodyConstraints.FreezePositionY;
        //Destroy(Eyes.gameObject);
        yield return new WaitForSeconds(2f);
        /*while (dissolveAmount < 1f)
        {
            dissolveAmount += Time.deltaTime * dissolveSpeed;
            SetDissolveAmount(dissolveAmount);
            yield return null;
        }
        */

        Destroy(gameObject);
    }
    /*
    private void SetDissolveAmount(float amount)
    {
        mpb.SetFloat("_DissolveAmount", amount);
        enemyRenderer.SetPropertyBlock(mpb);
    }
    */
}
