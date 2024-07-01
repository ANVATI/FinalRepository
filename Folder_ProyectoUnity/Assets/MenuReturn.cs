using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuReturn : MonoBehaviour
{
    //EL GAMEMANAGER ES UNA NENA QUE PARA MOSTRANDO ERRORES >:C
    private void OnEnable()
    {
        Boss.onBossDead += ReturnMenúAfterWin;
    }
    private void OnDisable()
    {
        Boss.onBossDead -= ReturnMenúAfterWin;
    }
    public void ReturnMenúAfterWin()
    {
        StartCoroutine(TimeToReturnMenú());
    }
    IEnumerator TimeToReturnMenú()
    {
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene("Menú");
        Time.timeScale = 1;
    }
}
