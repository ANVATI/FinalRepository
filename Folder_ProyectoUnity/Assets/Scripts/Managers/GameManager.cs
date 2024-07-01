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
    public void ReturnMen�Die()
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
        SceneManager.LoadScene("Men�");
        Time.timeScale = 1;

    }
    public void RestartLvl()
    {
        SceneManager.LoadScene("Nivel");
        Time.timeScale = 1;
    }
    public void ReturnMen�()
    {
        SceneManager.LoadScene("Men�");
        Time.timeScale = 1;
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
