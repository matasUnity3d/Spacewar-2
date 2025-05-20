using UnityEngine;
using System.Collections;
public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource thrusterSource;
    [SerializeField] AudioSource sfxSource;

    public AudioClip Death;
    public AudioClip PlanetDestroy;
    public AudioClip FuelUp;
    public AudioClip Boost;
    public AudioClip Music;
    public AudioClip Lazer;
    public AudioClip Thruster;

    private void Start()
    {
        musicSource.clip = Music;
        musicSource.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
    public void PlayThrusters(bool Toggle)
    {
        if (Toggle)
        {
            if (thrusterSource.isPlaying) return;
            Debug.Log("thruster");
            thrusterSource.clip = Thruster;
            thrusterSource.Play();
        }
        else thrusterSource.Stop();
    }
}

