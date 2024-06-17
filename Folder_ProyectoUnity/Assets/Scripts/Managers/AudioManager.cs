using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; set; }
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider sliderMaster;
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderSFX;
    [SerializeField] private AudioSettings audioSettings;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        LoadVolumeSettings();
    }

    private void LoadVolumeSettings()
    {
        sliderMaster.value = audioSettings.masterVolume;
        sliderMusic.value = audioSettings.musicVolume;
        sliderSFX.value = audioSettings.sfxVolume;
        SetVolumeMaster();
        SetVolumeMusic();
        SetVolumeSFX();
    }

    public void SetVolumeMaster()
    {
        float volume = sliderMaster.value;
        myMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        audioSettings.masterVolume = volume;
    }

    public void SetVolumeMusic()
    {
        float volume = sliderMusic.value;
        myMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        audioSettings.musicVolume = volume;
    }

    public void SetVolumeSFX()
    {
        float volume = sliderSFX.value;
        myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        audioSettings.sfxVolume = volume;
    }
}
