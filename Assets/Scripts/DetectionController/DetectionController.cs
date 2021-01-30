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
    }

    public void CharacterDetected(float distance)
    {
        Value += _detectionData.increasePerDistance * distance;

        int newAlertLevel = GetAlertLevelBaseOnValue();
        if (AlertLevel != newAlertLevel)
        {
            AlertLevel = newAlertLevel;
        }
        
        Debug.Log($"Value: {_value}  distance: {distance}");
        
        if (Value >= _detectionData.maxValueThirdState)
        {
            _levelController.GameOverLevel();
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
