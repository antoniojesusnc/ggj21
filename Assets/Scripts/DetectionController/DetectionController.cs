using UnityEngine;

public class DetectionController : MonoBehaviour
{
    [SerializeField] private DetectionData _detectionData;
    
    LevelController _levelController;

    private float Value => _value;
    private float _value; 
    
    void Start()
    {
        _levelController = FindObjectOfType<LevelController>();
        _levelController.AddDisableWhenLevelComplete(this);
    }

    public void CharacterDetected(float distance)
    {
        _value += _detectionData.increasePerDetection;

        Mathf.Clamp(_value, 0, _detectionData.maxValue);
        if (_value >= _detectionData.maxValue)
        {
            _levelController.GameOverLevel();
        }
    }

    void Update()
    {
        _value -= _detectionData.decreasePerSeconds * Time.deltaTime;
    }
}
