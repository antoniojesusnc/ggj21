using UnityEngine;

public class CharacterInput : MonoBehaviour
{
    public const string XAxisName = "Horizontal";
    public const string YAxisName = "Vertical";
    
    public CharacterMovementStatus CurrentStatus { get; private set; }
    void Update()
    {
        GetKeyboardInput();
    }

    private void GetKeyboardInput()
    {
        float axisX = Input.GetAxis(XAxisName);  
        if (axisX != 0)
        {
            if (axisX < 0)
            {
                CurrentStatus = CharacterMovementStatus.LEFT;
            }else
            {
                CurrentStatus = CharacterMovementStatus.RIGHT;
            }
        }
        
        float axisY = Input.GetAxis(YAxisName);  
        if (axisY != 0)
        {
            if (axisY < 0)
            {
                CurrentStatus = CharacterMovementStatus.DOWN;
            }else
            {
                CurrentStatus = CharacterMovementStatus.UP;
            }
        }

        if (axisX != 0 && axisY != 0)
        {
            CurrentStatus = 0;
        }
    }
}
