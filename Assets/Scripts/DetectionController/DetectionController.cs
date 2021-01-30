using UnityEngine;

public class DetectionController : MonoBehaviour
{
    [SerializeField] private DetectionData _detectionData;

    LevelController _levelController;

    public int AlertLevel { get; private set; } 
    public float Value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
            _value = Mathf.Clamp(_value, 0, _detectionData.maxValueThirdState);
        }
    }
    private float _value; 
    
    
    void Start()
    {
        _levelController = FindObjectOfType<LevelController>();
        _levelController.AddDisableWhenLevelComplete(this);
        SetNewALert(1);
    }

    public void CharacterDetected(float distance)
    {
        Value += _detectionData.increasePerDistance * distance*Time.deltaTime;

        int newAlertLevel = GetAlertLevelBaseOnValue();
        if (AlertLevel != newAlertLevel)
        {
            SetNewALert(newAlertLevel);
        }
        
        Debug.Log($"Value: {_value}  distance: {distance}");
        
        if (Value >= _detectionData.maxValueThirdState)
        {
            _levelController.GameOverLevel();
        }
    }

    private void SetNewALert(int newAlertLevel)
    {
        SetAudio(newAlertLevel);
        AlertLevel = newAlertLevel;
    }

    private void SetAudio(int newAlertLevel)
    {
        var audioType = GetSoundByAlert(AlertLevel);
        AudioController.Instance.StopSound(audioType);
        
        audioType = GetSoundByAlert(newAlertLevel);
        AudioController.Instance.PlaySound(audioType);
    }

    private EAudioType GetSoundByAlert(int alertLevel)
    {
        switch (alertLevel)
        {
            case 1: return EAudioType.BarLvl1;
            case 2: return EAudioType.BarLvl2;
            case 3: return EAudioType.BarLvl3;
            default: return EAudioType.None;
        }
    }

    private int GetAlertLevelBaseOnValue()
    {
        if (Value < _detectionData.maxValueFirstState)
        {
            return 0;
        }
        else if(Value < _detectionData.maxValueSecondState)
        {
            return 1;
        }
        else if(Value < _detectionData.maxValueSecondState)
        {
            return 2;
        }
        else
        {
            return 3;
        }
    }
    
    void Update()
    {
        Value -= _detectionData.decreasePerSeconds * Time.deltaTime;
    }
}
