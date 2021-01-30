using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    [SerializeField]
    private EAudioType _audioType;
    void Start()
    {
        AudioController.Instance.PlaySound(_audioType);
    }
}
