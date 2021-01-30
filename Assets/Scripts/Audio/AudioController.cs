using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;
    
    [SerializeField]
    private AudioData _audioData;

    private List<AudioSource> _audioSource;

    void Start()
    {
        Instance = this;
        _audioSource = new List<AudioSource>();
    }

    public void StopAllSounds()
    {
        for (int j = 0; j < _audioSource.Count; j++)
        {
            _audioSource[j].Stop();
        }
    }
    
    public void StopSound(EAudioType audioType)
    {
        if (audioType == EAudioType.None)
        {
            return;
        }
        StopSoundInternal(GetAudioData(audioType));
    }

    private void StopSoundInternal(AudioDataInfo audioDataAudio)
    {
        for (int i = 0; i < audioDataAudio.audios.Count; i++)
        {
            for (int j = 0; j < _audioSource.Count; j++)
            {
                if (audioDataAudio.audios[i] == _audioSource[j].clip)
                {
                    _audioSource[j].Stop();
                    break;
                }
            }
        }
    }

    public void PlaySound(EAudioType audioType)
    {
        if (audioType == EAudioType.None)
        {
            return;
        }

        if (IsPlaying(audioType))
        {
            return;
        }

        var audioData = GetAudioData(audioType);
        PlaySoundInternal(audioData);
    }

    private AudioDataInfo GetAudioData(EAudioType audioType)
    {
        for (int i = 0; i < _audioData.audios.Count; i++)
        {
            if (_audioData.audios[i].type == audioType &&
                _audioData.audios[i].audios.Count > 0)
            {
                return _audioData.audios[i];
            }
        }

        return null;
    }

    private bool IsPlaying(EAudioType audioType)
    {
        var audioData = GetAudioData(audioType);
        for (int j = 0; j < _audioSource.Count; j++)
        {
            for (int k = 0; k < audioData.audios.Count; k++)
            {
                if (_audioSource[j].isPlaying && 
                    audioData.audios[k] == _audioSource[j].clip)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void PlaySoundInternal(AudioDataInfo audioInfo)
    {
        var randomClipIndex = Random.Range(0, audioInfo.audios.Count);
        var audioClip = audioInfo.audios[randomClipIndex];
        
        var audioSource = GetAudioSource();
        audioSource.clip = audioClip;
        audioSource.Play();
        audioSource.loop = audioInfo.isLoop;
        audioSource.volume = audioInfo.volume;
    }

    private AudioSource GetAudioSource()
    {
        AudioSource audioSource = null;
        for (int i = 0; i < _audioSource.Count; i++)
        {
            if (!_audioSource[i].isPlaying)
            {
                audioSource = _audioSource[i];
                break;
            }
        }

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.Add(audioSource);
        }

        return audioSource;
    }
}
