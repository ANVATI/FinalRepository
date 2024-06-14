using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;

public class PlayerActions : MonoBehaviour
{
    public Action onRage;
    public PlayerAttributes attributes;
    public PlayerController playerController;
    private float remainingRageDuration = 0f;
    private int enemyKillCount = 0;
    public bool inRageMode;
    public AnimationCurve scaleCurve;
    private Coroutine rageCoroutine;

    private void Start()
    {
        if (playerController == null)
        {
            playerController = PlayerController.Instance;
        }
    }

    private void Awake()
    {
        // Pruebas
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

        if (rageCoroutine != null)
        {
            StopCoroutine(rageCoroutine);
        }
        rageCoroutine = StartCoroutine(ReduceRageOverTime(attributes.RageDuration));
    }

    private IEnumerator ReduceRageOverTime(float duration)
    {
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
}
