using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viking : HerenciaEnemy
{
    public LibrarySounds vikingSounds;
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
        StartCoroutine(DieViking());
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

    IEnumerator DieViking()
    {
        animator.SetTrigger("VikingDie");
        enemyCollider.enabled = false;
        yield return new WaitForSeconds(0.1f);
        rb.constraints |= RigidbodyConstraints.FreezePositionY;
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
    
}
