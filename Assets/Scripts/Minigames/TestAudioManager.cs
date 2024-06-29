using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestAudioManager : MonoBehaviour
{
    // this manages the audio in the minigame test scene

    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioClip[] spookyMusic = new AudioClip[2];
    // 0: during all minigames except 1B
    // 1: during 1B
    [SerializeField] private AudioSource _sfxSource;

    [SerializeField] private AudioClip[] errorSounds = new AudioClip[4];
    // 0: computer glitch
    // 1: weird
    // 2: footsteps
    // 3: knocking

    [SerializeField] private AudioClip[] toolSounds = new AudioClip[2];
    // 0: welding
    // 1: hammer

    [SerializeField] private AudioClip glitchSound;
    [SerializeField] private AudioClip weirdSound;

    public delegate void OnGlitch(int index);
    public static OnGlitch onGlitch;

    public delegate void OnUsingTool(int index);
    public static OnUsingTool onUsingTool;


    void Start()
    {
        _musicSource.clip = spookyMusic[0];
        _musicSource.Play();

        onGlitch += PlaySFX;
        onUsingTool += PlayToolSFX;
    }

    private void PlaySFX(int index)
    {
        _sfxSource.PlayOneShot(errorSounds[index], _sfxSource.volume);
    }

    private void PlayToolSFX(int index)
    {
        if (!_sfxSource.isPlaying)
        {
            _sfxSource.PlayOneShot(toolSounds[index], _sfxSource.volume);
        }
    }

    public void PlayOutsideMusic(bool is1B)
    {
        if (is1B && _musicSource.clip == spookyMusic[0])
        {
            _musicSource.clip = spookyMusic[1];
            _musicSource.Play();
        }
        if (!is1B && _musicSource.clip == spookyMusic[1])
        {
            _musicSource.clip = spookyMusic[0];
            _musicSource.Play();
        }
    }
}
