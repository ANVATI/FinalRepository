using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    public Image LifeBar;
    //public Image bossLifeBar;
    //public Image vikingLifeBar;
    public GameObject UI;

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

        if (goblin != null && LifeBar != null)
        {
            UpdateGoblinLifeBar();
        }

        if (boss != null && LifeBar != null)
        {
            UpdateBossLifeBar();
        }
        if (viking != null && LifeBar != null)
        {
            UpdateVikingLifeBar();
        }
    }

    void UpdateGoblinLifeBar()
    {
        float fillAmount = (float)goblin.GetCurrentHP() / (float)goblin.GetMaxHP();
        LifeBar.fillAmount = fillAmount;
        if (fillAmount <= 0.01f)
        {
            UI.SetActive(false);
        }
    }

    void UpdateBossLifeBar()
    {
        float fillAmount = (float)boss.GetCurrentHP() / (float)boss.GetMaxHP();
        LifeBar.fillAmount = fillAmount;
        if (fillAmount <= 0.01f)
        {
            UI.SetActive(false);
        }
    }
    void UpdateVikingLifeBar()
    {
        float fillAmount = (float)viking.GetCurrentHP() / (float)viking.GetMaxHP();
        LifeBar.fillAmount = fillAmount;
        if (fillAmount <= 0.01f)
        {
            UI.SetActive(false);
        }
    }

}