using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    public Image goblinLifeBar;
    public Image bossLifeBar;
    public Image vikingLifeBar;

    private Goblin goblin;
    private Boss boss;
    private Viking viking;

    private void Start()
    {
        goblin = GetComponentInParent<Goblin>();
        boss = GetComponentInParent<Boss>();
        viking = GetComponentInParent<Viking>();
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
        if (viking != null && vikingLifeBar != null)
        {
            UpdateVikingLifeBar();
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
    void UpdateVikingLifeBar()
    {
        float fillAmount = (float)viking.GetCurrentHP() / (float)viking.GetMaxHP();
        vikingLifeBar.fillAmount = fillAmount;
    }
}