using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : Singleton<SoundFXManager>
{
    [SerializeField]
    private AudioClip[] _audioClips;

    [SerializeField] private GameObject _soundFXObject;


    private GameObject _soundFXParent;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        _soundFXParent = GameObject.Find("SoundFX");
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform)
    {
        GameObject soundFXGameObject = Instantiate(_soundFXObject, spawnTransform.position, Quaternion.identity);

        AudioSource audioSource = soundFXGameObject.GetComponent<AudioSource>();

        audioSource.clip = audioClip;



        soundFXGameObject.GetComponent<SoundEffect>().PlaySound();

        float destroyTime = audioClip.length + 0.1f; // Add a small buffer to ensure the sound finishes playing

        Destroy(soundFXGameObject, destroyTime);

    }

    public void PlaySoundFX(int typeEffect, Transform spawnTransform)
    {
        GameObject soundFXGameObject = Instantiate(_soundFXObject, spawnTransform.position, Quaternion.identity);
        soundFXGameObject.transform.parent = _soundFXParent.transform; // Ensure the sound effect is parented to the SoundFX GameObject

        AudioSource audioSource = soundFXGameObject.GetComponent<AudioSource>();

        audioSource.clip = _audioClips[typeEffect];

        soundFXGameObject.GetComponent<SoundEffect>().PlaySound();

        float destroyTime = _audioClips[typeEffect].length + 0.1f; // Add a small buffer to ensure the sound finishes playing

        Destroy(soundFXGameObject, destroyTime);
    }

    public void PlaySoundFX(string nameSound, Transform spawnTransform)
    {
        List<AudioClip> validClips = new();

        if (_audioClips == null || _audioClips.Length == 0)
        {
            Debug.LogError("SFXManager: AudioClips not loaded!!!");
        }


        for (int i = 0; i < _audioClips.Length; i++)
        {
            if (_audioClips[i].name.ToLowerInvariant().Contains(nameSound.ToLowerInvariant()))
            {
                validClips.Add(_audioClips[i]);
            }
        }
        if (validClips.Count == 0)
        {
            Debug.LogWarning("No sound clips found with the name: " + nameSound);
            return;
        }

        int count = validClips.Count;
        int randomIndex = Random.Range(0, count);

        int index = System.Array.IndexOf(_audioClips, validClips[randomIndex]);

        PlaySoundFX(index, spawnTransform);

    }
    public void PlaySoundClick()
    {
        PlaySoundFX(0, transform); // Assuming the click sound is at index 0 in the _audioClips array
    }

}
