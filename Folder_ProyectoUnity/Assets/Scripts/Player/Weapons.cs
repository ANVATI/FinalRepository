using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    public BoxCollider[] weapons;
    public GameObject[] activateWeapons;
    public GameObject trail1;
    public GameObject trail2;
    public GameObject trail3;
    private AudioSource _weaponsSource;
    public AudioClip slashSound;
    public int Damage = 5;
    private void Awake()
    {
        _weaponsSource = GetComponents<AudioSource>()[0];
    }
    private void Start()
    {
        DesactivarColliders();
    }
    public void ActivarColliders()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] != null)
            {
                weapons[i].enabled = true;
            }
        }
        _weaponsSource.PlayOneShot(slashSound);
        trail1.SetActive(true);
        trail2.SetActive(true);
        trail3.SetActive(true);
        Debug.Log("Se activo el arma");

    }
    public void DesactivarColliders()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] != null)
            {
                weapons[i].enabled = false;
            }
        }
        trail1.SetActive(false);
        trail2.SetActive(false);
        trail3.SetActive(false);
        Debug.Log("Se desactivo el arma");
    }
    public void ActivateWeapons(int index)
    {
        for (int i = 0; i < activateWeapons.Length; i++)
        {
            activateWeapons[i].SetActive(false);
        }
        activateWeapons[index].SetActive(true);
    }
}
