using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestAudioManager : MonoBehaviour
{
    // this manages the audio in the minigame test scene

    //[SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;
    //[SerializeField] private AudioClip spookyMusic01;
    //[SerializeField] private AudioClip spookyMusic02;
    [SerializeField] private AudioClip glitchSound;
    [SerializeField] private AudioClip weirdSound;
    public delegate void OnGlitch(bool glitch);
    public static OnGlitch onGlitch;

    //[SerializeField] private Slider musicSlider;
    //[SerializeField] private Slider sfxSlider;


    void Start()
    {
        //_musicSource.Play();

        onGlitch += PlaySFX;
    }

    private void PlaySFX(bool glitch)
    {
        if (glitch)
        {
            _sfxSource.PlayOneShot(glitchSound, _sfxSource.volume);
        }
        else
        {
            _sfxSource.PlayOneShot(weirdSound, _sfxSource.volume);
        }
    }

    /*
    public void UpdateVolume(GameObject slider)
    {

    }
    */
}
