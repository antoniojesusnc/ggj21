using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MessageData", menuName = "Data/New Message Data", order = 1)]
public class MessageData : ScriptableObject
{
    public float fade;
    public List<MessageDataInfo> messages;
}

[System.Serializable]
public class MessageDataInfo
{
    public EMessageType type;
    public string text;
    public float duration;
    public bool onlyShowOnce;
    public EMessageType triggerMessage;
}

public enum EMessageType
{
    None,
    InitialMessage,
    CameraMessage,
    StartMessage,
    FirstCollectable,
    AllCollectable,
    LevelComplete,
    InitialMessage2,
    InvisibleMessage,
    InitialMessage3,
    CameraMessage2
}
