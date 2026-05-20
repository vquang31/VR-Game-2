using UnityEngine;

public class SoundFXMananger : Singleton<SoundFXMananger>
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
}
