using UnityEngine;

public static class SoundManager
{
    public const string MUSIC_VOLUME = "MusicVolume";
    public const string FX_VOLUME = "FXVolume";

    public static float MusicVolume { get; set; }
    public static float FXVolume { get; set; }

    public static void Init()
    {
        if (PlayerPrefs.HasKey(MUSIC_VOLUME))
        {
            MusicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME);
        }
        else
        {
            MusicVolume = 0.25f;
        }

        if (PlayerPrefs.HasKey(FX_VOLUME))
        {
            FXVolume = PlayerPrefs.GetFloat(FX_VOLUME);
        }
        else
        {
            FXVolume = 0.25f;
        }
    }

    public static void PlaySound(AudioSource source, AudioClip clip, float volume)
    {
        source.PlayOneShot(clip, volume);
    }

    public static void PlayMusic(AudioSource source, AudioClip clip, float volume)
    {
        source.loop = true;
        source.PlayOneShot(clip, volume);
    }

    public static void ChangeMusicVolume(AudioSource source)
    {
        source.volume = MusicVolume;
    }
}
