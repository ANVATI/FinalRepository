using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    public Image goblinLifeBar;
    public Image bossLifeBar;

    private Goblin goblin;
    private Boss boss;

    private void Start()
    {
        goblin = GetComponentInParent<Goblin>();
        boss = GetComponentInParent<Boss>();
    }

    private void Update()
    {
        transform.forward = Camera.main.transform.forward;

        if (goblin != null && goblinLifeBar != null)
        {
            UpdateGoblinLifeBar();
        }

        if (boss != null && bossLifeBar != null)
        {
            UpdateBossLifeBar();
        }
    }

    void UpdateGoblinLifeBar()
    {
        float fillAmount = (float)goblin.GetCurrentHP() / (float)goblin.GetMaxHP();
        goblinLifeBar.fillAmount = fillAmount;
    }

    void UpdateBossLifeBar()
    {
        float fillAmount = (float)boss.GetCurrentHP() / (float)boss.GetMaxHP();
        bossLifeBar.fillAmount = fillAmount;
    }
}