using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public GameObject Options;

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
    public void AppearOptions()
    {
        
    }
    public void DissapearOptions()
    {
        Options.SetActive(false);
    }

    IEnumerator WaitForChangeScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Nivel");
    }


}
