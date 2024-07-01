using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    //[SerializeField] PlayerActions playeractions;
    //[SerializeField] GameObject wall;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    private void OnEnable()
    {
        OptionsMenuController.OnImagesMoved += ChangeScene;
        //playeractions.onPlayerEnterBossArea += CloseCave;
    }

    private void OnDisable()
    {
        OptionsMenuController.OnImagesMoved -= ChangeScene;
        //playeractions.onPlayerEnterBossArea -= CloseCave;
    }
    public void ChangeScene()
    {
        StartCoroutine(WaitForChangeScene());
    }
    /*
    public void CloseCave(bool close)
    {
        if (close)
        {
            wall.SetActive(true);
        }
    }
    */
    public void ReturnMenúDie()
    {
        StartCoroutine(WaitForReturn());
    }
    IEnumerator WaitForChangeScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Nivel");
    }

    IEnumerator WaitForReturn()
    {
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene("Menú");
        Time.timeScale = 1;

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
