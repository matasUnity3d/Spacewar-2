using UnityEngine;
using System.Collections;
public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    public AudioClip Death;
    public AudioClip PlanetDestroy;
    public AudioClip FuelUp;
    public AudioClip Boost;
    public AudioClip Music;
    public AudioClip Lazer;

    private void Start()
    {
        musicSource.clip = Music;
        musicSource.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}

