using UnityEngine;

public class SoundEffect : NewMonoBehaviour
{

    [SerializeField] private AudioSource m_AudioSource;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        m_AudioSource = GetComponent<AudioSource>();    
    }

    public void PlaySound()
    {
        m_AudioSource.Play();
    }
}
