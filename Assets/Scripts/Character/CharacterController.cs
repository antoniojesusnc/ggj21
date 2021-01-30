using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public CharacterData CharacterData => _characterData;
    [SerializeField]
    private CharacterData _characterData;

    public Vector3 LookDirection { get; private set; }

    private ECharacterStatus _lastCharacterStatus;
    public ECharacterStatus CharacterStatus { get; private set; }


    public void SetLookDirection(Vector2 newLookDirection)
    {
        if (newLookDirection != Vector2.zero)
        {
            LookDirection = newLookDirection;
        }
    }
    
    public void ChangeCharacterStatus(ECharacterStatus newCharacterStatus)
    {
        if (newCharacterStatus != CharacterStatus)
        {
            _lastCharacterStatus = CharacterStatus;
            CharacterStatus = newCharacterStatus;
        }
    }
}
