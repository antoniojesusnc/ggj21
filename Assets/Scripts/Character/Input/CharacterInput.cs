using UnityEngine;

public class CharacterInput : MonoBehaviour, ICharacterInput
{
    public const string XAxisName = "Horizontal";
    public const string YAxisName = "Vertical";
    
    public ECharacterInput CurrentInput { get; private set; }
    public bool HasMovement => CurrentInput != ECharacterInput.NONE;
    void Update()
    {
        GetKeyboardInput();
    }

    private void GetKeyboardInput()
    {
        CurrentInput = ECharacterInput.NONE;
        
        float axisX = Input.GetAxis(XAxisName);  
        if (axisX != 0)
        {
            if (axisX < 0)
            {
                CurrentInput = ECharacterInput.LEFT;
            }else
            {
                CurrentInput = ECharacterInput.RIGHT;
            }
        }
        
        float axisY = Input.GetAxis(YAxisName);  
        if (axisY != 0)
        {
            if (axisY < 0)
            {
                CurrentInput = ECharacterInput.DOWN;
            }else
            {
                CurrentInput = ECharacterInput.UP;
            }
        }

        if (axisX != 0 && axisY != 0)
        {
            CurrentInput = 0;
        }
    }
}
