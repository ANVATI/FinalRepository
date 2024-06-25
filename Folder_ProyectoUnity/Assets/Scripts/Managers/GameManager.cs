using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    private void OnEnable()
    {
        OptionsMenuController.OnImagesMoved += ChangeScene;
    }

    private void OnDisable()
    {
        OptionsMenuController.OnImagesMoved -= ChangeScene;
    }
    public void ChangeScene()
    {
        StartCoroutine(WaitForChangeScene());
    }

    IEnumerator WaitForChangeScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Nivel");
    }
    public void RestartLvl()
    {
        SceneManager.LoadScene("Nivel");
        Time.timeScale = 1;
    }
    public void ReturnMenú()
    {
        SceneManager.LoadScene("Menú");
        Time.timeScale = 1;
    }

}
