using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] List<AudioData> sfxList;

    [SerializeField] AudioSource musicPlayer;
    [SerializeField] AudioSource sfxPlayer;

    [SerializeField] float fadeDuration = 0.75f;

    AudioClip currentMusic;
    float originalMusicVol;

    Dictionary<AudioID, AudioData> sfxLookUp;

    public static AudioManager i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    private void Start()
    {
        originalMusicVol = musicPlayer.volume;

        sfxLookUp = sfxList.ToDictionary(x => x.id);
    }

    public void PlaySfx(AudioClip clip, bool pauseMusic = false)
    {
        if (clip == null) return;

        if (pauseMusic)
        {
            musicPlayer.Pause();
            StartCoroutine(UnPauseMusic(clip.length));
        }

        sfxPlayer.PlayOneShot(clip);
    }

    public void PlaySfx(AudioID audioId, bool pauseMusic=false)
    {
        if (!sfxLookUp.ContainsKey(audioId)) return;

        var audioData = sfxLookUp[audioId];
        PlaySfx(audioData.clip, pauseMusic);
    }



    public void PlayMusic(AudioClip clip, bool loop=true, bool fade=false)
    {
        if (clip == null || clip == currentMusic) return;

        currentMusic = clip;
        StartCoroutine(PlayMusicAsync(clip, loop, fade)); 
    }

    IEnumerator PlayMusicAsync(AudioClip clip, bool loop, bool fade)
    {
        if (fade)
        {
            yield return musicPlayer.DOFade(0, fadeDuration).WaitForCompletion();
        }

        musicPlayer.clip = clip;
        musicPlayer.loop = loop;
        musicPlayer.Play();

        if (fade)
        {
            yield return musicPlayer.DOFade(originalMusicVol, fadeDuration).WaitForCompletion();
        }
    }

    IEnumerator UnPauseMusic(float delay)
    {
        yield return new WaitForSeconds(delay);

        musicPlayer.volume = 0;
        musicPlayer.UnPause();
        musicPlayer.DOFade(originalMusicVol, fadeDuration);
    }
}

public enum AudioID {  UISelect, ItemObtained, FirstThunder, ThunderHit }

[System.Serializable]
public class AudioData
{
    public AudioID id;
    public AudioClip clip;
}
