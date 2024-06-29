using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChanger : MonoBehaviour
{

    public AudioSource _audio;
    bool isOn;
    public float minVolume = 0;
    public float maxValue = 0.5f;
    public float speedUpdate = 0.1f;

    private void Update()
    {
        if (isOn)
        {
            _audio.volume = Mathf.Lerp(_audio.volume, maxValue, speedUpdate);
        }
        else
        {
            _audio.volume = Mathf.Lerp(_audio.volume, minVolume, speedUpdate);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isOn = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isOn = false;
        }
    }
}
