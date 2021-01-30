using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public CharacterData CharacterData => _characterData;
    [SerializeField]
    private CharacterData _characterData;

    public Vector3 LookDirection { get; private set; }
    
    public ECharacterStatus LastCharacterStatus { get; private set; }
    public ECharacterStatus CharacterStatus { get; private set; }


    public void SetLookDirection(Vector2 newLookDirection)
    {
        if (newLookDirection != Vector2.zero)
        {
            LookDirection = newLookDirection;
        }
    }

    public virtual void ChangeCharacterStatus(ECharacterStatus newCharacterStatus)
    {
        if (newCharacterStatus != CharacterStatus)
        {
            LastCharacterStatus = CharacterStatus;
            CharacterStatus = newCharacterStatus;
        }
    }
}
