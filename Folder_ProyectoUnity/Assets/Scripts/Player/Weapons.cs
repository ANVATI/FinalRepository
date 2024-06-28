using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    public BoxCollider[] weapons;
    public GameObject[] activateWeapons;
    public GameObject[] trails; 

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
        DesactivarTrails(); 
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
        ActivarTrails();
        Debug.Log("Se activó el arma");
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
        DesactivarTrails(); 
        Debug.Log("Se desactivó el arma");
    }

    private void ActivarTrails()
    {
        for (int i = 0; i < trails.Length; i++)
        {
            if (trails[i] != null)
            {
                trails[i].SetActive(true);
            }
        }
    }

    private void DesactivarTrails()
    {
        for (int i = 0; i < trails.Length; i++)
        {
            if (trails[i] != null)
            {
                trails[i].SetActive(false);
            }
        }
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
