using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : NewMonoBehaviour
{
    [SerializeField]
    private AudioMixer _audioMixer;


    protected override void LoadComponents()
    {
        base.LoadComponents();
    }

    public void SetMasterVolume(float level)
    {
        _audioMixer.SetFloat("MasterVolume", Mathf.Log10(level) * 20f);
    }

    public void SetMusicVolume(float level)
    {
        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(level) * 20f);
    }

    public void SetSFXVolume(float level)
    {
        _audioMixer.SetFloat("SFXVolume", Mathf.Log10(level) * 20f);
    }

}
