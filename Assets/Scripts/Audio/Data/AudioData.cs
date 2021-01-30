using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioData", menuName = "Data/New Audio Data", order = 1)]
public class AudioData : ScriptableObject
{
    public List<AudioDataInfo> audios;
}

[System.Serializable]
public class AudioDataInfo
{
    public EAudioType type;
    public List<AudioClip> audios;
    public bool isLoop;
    [Range(0,1)]
    public float volume = 0.7f;
}

public enum EAudioType
{
    None= -1,
    MainMusic,
    BarLvl1,
    BarLvl2,
    BarLvl3,
    GameOver,
    Victory,
    ThiefStep,
    GuardStep,
    SFXWall,
    SFXSteal,
    
}

