using UnityEngine;
using DG.Tweening;
using System;
using System.Collections;

public class PlayerActions : MonoBehaviour
{
    public Action onPlayerEnterBossArea;
    public Action onRage;
    public Action onKillAllGoblins;
    public Action onKillAllViking;

    public Action OnGoblinArea;
    public Action OnAldeaArea;

    private bool GlobinArea;
    private bool AldeaArea;

    public PlayerAttributes attributes;
    public GameManager manager;
    public PlayerController playerController;
    public UIManager UI;
    private float remainingRageDuration = 0f;
    private int enemyKillCount;
    public bool inRageMode;
    public AnimationCurve scaleCurve;
    public float currentHP;
    public bool inZone = false;
    private AudioSource _audioSource;
    public LibrarySounds _actionSounds;
    public GameObject rageEffect;

    private void Awake()
    {
        rageEffect.SetActive(false);
        Debug.Log("Valores Restaurados");
        attributes.maxSpeed = 10f;
        attributes.acceleration = 2.5f;
        attributes.currentSpeed = 2f;
        attributes.Life = 200f;
        attributes.walkSpeed = 5.0f;
        attributes.crouchSpeed = 3.0f;
        attributes.rollForce = 7.5f;
        attributes.Stamina = 100f;
        attributes.runSpeed = 10f;
        currentHP = attributes.Life;
        _audioSource = GetComponents<AudioSource>()[0];
        inZone = false;
        playerController = PlayerController.Instance;
        manager = FindObjectOfType<GameManager>();
        UI = FindObjectOfType<UIManager>();
    }

    public float GetRemainingRageDuration()
    {
        return remainingRageDuration / attributes.RageDuration * 10;
    }

    private void OnEnable()
    {
        HerenciaEnemy.OnEnemyKilled += IncrementEnemyKillCount;
        onRage += ApplyRageEffect;
    }

    private void OnDisable()
    {
        HerenciaEnemy.OnEnemyKilled -= IncrementEnemyKillCount;
        onRage -= ApplyRageEffect;
    }

    public void TriggerRage()
    {
        if (enemyKillCount >= 10 && !inRageMode)
        {
            onRage?.Invoke();
        }
    }

    private void ApplyRageEffect()
    {
        playerController.playerAttributes.Stamina = 100f;
        ScaleForDuration(Vector3.one * 1.5f, attributes.RageDuration);
        StartCoroutine(ReduceRageOverTime(attributes.RageDuration));
    }

    private IEnumerator ReduceRageOverTime(float duration)
    {
        rageEffect.SetActive(true);
        float startValue = 10;
        float time = 0;

        while (time < duration)
        {
            remainingRageDuration = Mathf.Lerp(startValue, 0, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        remainingRageDuration = 0;
        inRageMode = false;
        rageEffect.SetActive(false);
        enemyKillCount = 0;
    }

    private void ScaleForDuration(Vector3 targetScale, float duration)
    {
        Debug.Log("MODO RAGE");
        Vector3 originalScale = transform.localScale;
        inRageMode = true;

        transform.DOScale(targetScale, duration).SetEase(scaleCurve).OnComplete(() =>
        {
            attributes.ApplyRageAttributes();
            transform.DOScale(originalScale, 4f).SetEase(scaleCurve).OnComplete(() =>
            {
                Debug.Log("Final del rage");
                transform.localScale = originalScale;
                attributes.ResetAttributes();
                playerController.playerAttributes.Stamina = 100f;
                inRageMode = false;
                enemyKillCount = 0;
            });
        });
    }

    public int GetEnemyKillCount()
    {
        return enemyKillCount;
    }

    public void IncrementEnemyKillCount()
    {
        if (enemyKillCount < 10)
        {
            enemyKillCount++;
        }
    }

    public void TakeDamageFromBoss(float bossDamage)
    {
        if (playerController.isDead) return;
        currentHP -= bossDamage;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (playerController.isDead) return; 
        playerController.PlayerDead();
        manager.ReturnMenúDie();
        UI.Fade();
        UI.ShowImageLose();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BossHand"))
        {
            Boss boss = other.gameObject.GetComponentInParent<Boss>();
            if (boss != null)
            {
                TakeDamageFromBoss(boss.damage);
                _audioSource.PlayOneShot(_actionSounds.clipSounds[UnityEngine.Random.Range(6, 8)]);
            }
        }
        else if (other.gameObject.CompareTag("BossArea"))
        {
            Boss boss = other.gameObject.GetComponentInParent<Boss>();
            if (boss != null)
            {
                TakeDamageFromBoss(boss.damage * 2f);
                _audioSource.PlayOneShot(_actionSounds.clipSounds[UnityEngine.Random.Range(6, 8)]);
            }
        }
        else if (other.gameObject.CompareTag("Zone"))
        {
            inZone = true;
            Destroy(other.gameObject);
            onPlayerEnterBossArea?.Invoke();
        }
        else if (other.gameObject.CompareTag("Bosque") && !GlobinArea)
        {
            OnGoblinArea?.Invoke();
            GlobinArea = true;
        }
        else if (other.gameObject.CompareTag("Aldea") && !AldeaArea)
        {
            OnAldeaArea?.Invoke();
            AldeaArea = true;
        }
        else if (other.gameObject.CompareTag("GoblinWeapon"))
        {
            Goblin goblin = other.gameObject.GetComponentInParent<Goblin>();
            if (goblin != null)
            {
                TakeDamageFromBoss(goblin.damage);
                _audioSource.PlayOneShot(_actionSounds.clipSounds[UnityEngine.Random.Range(6, 8)]);
            }
        }
        else if (other.gameObject.CompareTag("VikingWeapon"))
        {
            Viking viking = other.gameObject.GetComponentInParent<Viking>();
            if (viking != null)
            {
                TakeDamageFromBoss(viking.damage);
                _audioSource.PlayOneShot(_actionSounds.clipSounds[UnityEngine.Random.Range(6, 8)]);

            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            TakeDamageFromBoss(0.01f); 
        }
    }
}
