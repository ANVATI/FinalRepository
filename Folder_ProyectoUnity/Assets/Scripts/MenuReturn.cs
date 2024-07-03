using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuReturn : MonoBehaviour
{
    //EL GAMEMANAGER ES UNA NENA QUE PARA MOSTRANDO ERRORES >:C
    private void OnEnable()
    {
        Boss.onBossDead += ReturnMen�AfterWin;
    }
    private void OnDisable()
    {
        Boss.onBossDead -= ReturnMen�AfterWin;
    }
    public void ReturnMen�AfterWin()
    {
        StartCoroutine(TimeToReturnMen�());
    }
    IEnumerator TimeToReturnMen�()
    {
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene("Men�");
        Time.timeScale = 1;
    }
}
