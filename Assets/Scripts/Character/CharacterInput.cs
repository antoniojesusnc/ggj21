using UnityEngine;

public class CharacterInput : MonoBehaviour
{
    public const string XAxisName = "Horizontal";
    public const string YAxisName = "Vertical";
    
    public ECharacterMovementStatus CurrentStatus { get; private set; }
    public bool HasMovement => CurrentStatus != ECharacterMovementStatus.NONE;
    void Update()
    {
        GetKeyboardInput();
    }

    private void GetKeyboardInput()
    {
        CurrentStatus = ECharacterMovementStatus.NONE;
        
        float axisX = Input.GetAxis(XAxisName);  
        if (axisX != 0)
        {
            if (axisX < 0)
            {
                CurrentStatus = ECharacterMovementStatus.LEFT;
            }else
            {
                CurrentStatus = ECharacterMovementStatus.RIGHT;
            }
        }
        
        float axisY = Input.GetAxis(YAxisName);  
        if (axisY != 0)
        {
            if (axisY < 0)
            {
                CurrentStatus = ECharacterMovementStatus.DOWN;
            }else
            {
                CurrentStatus = ECharacterMovementStatus.UP;
            }
        }

        if (axisX != 0 && axisY != 0)
        {
            CurrentStatus = 0;
        }
    }
}
