using System;
using SimpleInputNamespace;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CharacterInput : MonoBehaviour, ICharacterInput
{
    public const string XAxisName = "Horizontal";
    public const string YAxisName = "Vertical";
    
    public ECharacterInput CurrentInput { get; private set; }
    public bool HasMovement => CurrentInput != ECharacterInput.NONE;

    
    public float XValue { get; private set; }
    public float YValue { get; private set; }

    private AxisInputUIArrows _mobileInputs;
    
    void Start()
    {
        DisableMeWhenLevelComplete();
        _mobileInputs = FindObjectOfType<AxisInputUIArrows>();
    }

    private void DisableMeWhenLevelComplete()
    {
        var levelController = FindObjectOfType<LevelController>();
        levelController.AddDisableWhenLevelComplete(this);
    }

    public void SetInput(float x, float y)
    {
        XValue = x;
        YValue = y;
    }

    private void OnDisable()
    {
        CurrentInput = ECharacterInput.NONE;
    }

    void Update()
    {
        ResetInputAxis();
        GetMobileInput();
        GetKeyboardInput();
        UpdateInput();
    }

    private void ResetInputAxis()
    {
        XValue = 0;
        YValue = 0;
    }

    private void GetMobileInput()
    {
        if (_mobileInputs != null && _mobileInputs.isActiveAndEnabled)
        {
            XValue = _mobileInputs.xAxis.value;
            YValue = _mobileInputs.yAxis.value;
        }
    }

    private void GetKeyboardInput()
    {
        var newXValue = Input.GetAxis(XAxisName);
        if (newXValue != 0)
        {
            XValue = newXValue;
        }
        var newYValue = Input.GetAxis(YAxisName);
        if (newYValue != 0)
        {
            YValue = newYValue;
        }
    }

    private void UpdateInput()
    {
        CurrentInput = ECharacterInput.NONE;
          
        if (XValue != 0)
        {
            if (XValue < 0)
            {
                CurrentInput = ECharacterInput.LEFT;
            }else
            {
                CurrentInput = ECharacterInput.RIGHT;
            }
        }
        
        if (YValue != 0)
        {
            if (YValue < 0)
            {
                CurrentInput = ECharacterInput.DOWN;
            }else
            {
                CurrentInput = ECharacterInput.UP;
            }
        }

        if (XValue != 0 && YValue != 0)
        {
            CurrentInput = 0;
        }
    }
}
