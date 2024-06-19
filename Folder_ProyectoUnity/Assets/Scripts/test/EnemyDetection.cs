using UnityEngine;
using System;

public class EnemyDetection : MonoBehaviour
{
    public Action startChasingPlayer;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (startChasingPlayer != null)
            {
                startChasingPlayer.Invoke();
            }
        }
    }
}
