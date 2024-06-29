using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockClose : MonoBehaviour
{
    [SerializeField] PlayerActions playeractions;
    [SerializeField] GameObject wall;
    AudioSource _audio;
    public AudioClip rockFalling;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        playeractions.onPlayerEnterBossArea += CloseCave;
    }

    private void OnDisable()
    {
        playeractions.onPlayerEnterBossArea -= CloseCave;
    }

    public void CloseCave()
    {
        _audio.PlayOneShot(rockFalling);
        wall.SetActive(true);

    }
}
