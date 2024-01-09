using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    // this script manages the sound effects and music
    // including the volume slider functionality

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    public float musicVolume;
    public float sfxVolume;
    private float fadeDuration = 10f;

    void Start()
    {
        musicVolume = 0.5f;
        sfxVolume = 0.5f;

        musicSlider.onValueChanged.AddListener(delegate { UpdateMusicVolume(); });
        sfxSlider.onValueChanged.AddListener(delegate { UpdateSfxVolume(); });
    }

    private void UpdateMusicVolume()
    {
        musicVolume = musicSlider.value;
    }

    private void UpdateSfxVolume()
    {
        sfxVolume = sfxSlider.value;
    }

    /*
    IEnumerator FadeIn(AudioSource musicSource, float fadeDuration)
    {
        musicSource.Play();
        musicSource.volume = 0f;
        while (musicSource.volume < musicVolume)
        {
            musicSource.volume += Time.deltaTime / fadeDuration;
            print(musicSource.volume);
            yield return null;
        }
    }

    IEnumerator FadeOut(AudioSource musicSource, float fadeDuration)
    {
        musicSource.volume = musicVolume;
        while (musicSource.volume > 0)
        {
            musicSource.volume -= Time.deltaTime / fadeDuration;
            //print(musicSource.volume);
            yield return null;
        }
    }
    */
}
