using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateWeapons : MonoBehaviour
{
    public Weapons getWeapons;
    public int weaponIndex;

    private void Start()
    {
        getWeapons = GameObject.FindGameObjectWithTag("Player").GetComponent<Weapons>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            getWeapons.ActivateWeapons(weaponIndex);
            Destroy(gameObject);
        }
    }
}
