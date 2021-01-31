using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageController : MonoBehaviour
{
    public static MessageController Instance;
    
    [SerializeField] private MessageData _messageData;
    
    [SerializeField] public SpriteRenderer _messageBox;
    [SerializeField] public SpriteRenderer _messageBoxSpike;
    [SerializeField] public TextMeshPro _text;

    private HashSet<EMessageType> _messagesWatched;

    private MessageDataInfo _currentMessageData;
    private bool _showingMessage;
    private bool _fadeIn;
    private bool _fadeOut;
    
    private float _timestamp;
    void Start()
    {
        _messagesWatched = new HashSet<EMessageType>();

        FinishMessageBox();
        Instance = this;
    }

    public void ShowMessage(EMessageType messageType)
    {
        if (_showingMessage)
        {
            return;
        }
        
        var messageData = GetMessageData(messageType);
        if (messageData == null)
        {
            return;
        }

        ShowMessageInternal(messageData);
    }

    private void ShowMessageInternal(MessageDataInfo messageData)
    {
        if (messageData.onlyShowOnce && _messagesWatched.Contains(messageData.type))
        {
            return;
        }

        _currentMessageData = messageData;

        StartMessageBox();
    }
    
    void Update()
    {
        if (_showingMessage)
        {
            UpdateMessageBox();
        }
    }

    private void StartMessageBox()
    {
        _text.text = _currentMessageData.text;
        _timestamp = 0;
        _showingMessage = true;
        _fadeIn = true;
        _fadeOut = false;
        
        _messageBox.color = Color.clear;
        _messageBoxSpike.color = Color.clear;
        _text.color = Color.clear;
        
        _messageBox.gameObject.SetActive(true);
        _messageBoxSpike.gameObject.SetActive(true);
        _text.gameObject.SetActive(true);
    }

    private void UpdateMessageBox()
    {
        _timestamp += Time.deltaTime;
        
        if (_fadeIn)
        {
            UpdateMessageBoxFadeIn();
        }
        else if (_fadeOut)
        {
            UpdateMessageBoxFadeOut();
        }
        else
        {
            UpdateMessageBoxWaiting();
        }
    }
    
    private void UpdateMessageBoxFadeIn()
    {
        _messageBox.color = Color.Lerp(Color.clear, Color.white, _timestamp / _messageData.fade);
        _messageBoxSpike.color= Color.Lerp(Color.clear, Color.white, _timestamp / _messageData.fade);
        _text.color = Color.Lerp(Color.clear, Color.black, _timestamp / _messageData.fade);

        if (_timestamp > _messageData.fade)
        {
            _fadeIn = false;
        }
    }
    
    private void UpdateMessageBoxWaiting()
    {
        if (_timestamp > _messageData.fade + _currentMessageData.duration)
        {
            _fadeOut = true;
        }
    }

    private void UpdateMessageBoxFadeOut()
    {
        float fadeValue = _timestamp - _messageData.fade - _currentMessageData.duration;
        _messageBox.color = Color.Lerp(Color.white, Color.clear, fadeValue / _messageData.fade);
        _messageBoxSpike.color = Color.Lerp(Color.white, Color.clear, fadeValue / _messageData.fade);
        _text.color = Color.Lerp(Color.black, Color.clear, fadeValue / _messageData.fade);

        if (fadeValue > _messageData.fade)
        {
            FinishMessageBox();
        }
    }

    private void FinishMessageBox()
    {
        _timestamp = 0;
        _showingMessage = false;
        
        _messageBox.gameObject.SetActive(false);
        _messageBoxSpike.gameObject.SetActive(false);
        _text.gameObject.SetActive(false);

        if (_currentMessageData != null)
        {
            _messagesWatched.Add(_currentMessageData.type);
            if (_currentMessageData.triggerMessage != EMessageType.None)
            {
                ShowMessage(_currentMessageData.triggerMessage);
            }
        }
    }


    private MessageDataInfo GetMessageData(EMessageType messageType)
    {
        for (int i = 0; i < _messageData.messages.Count; i++)
        {
            if (_messageData.messages[i].type == messageType)
            {
                return _messageData.messages[i];
            }
        }

        return null;
    }
}
