using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    public BoxCollider[] weapons;
    public GameObject[] activateWeapons;
    private AudioSource _weaponsSource;
    public AudioClip slashSound;
    public int Damage = 3;
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
