using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public CharacterData CharacterData => _characterData;
    [SerializeField]
    private CharacterData _characterData;


    private ECharacterStatus _lastCharacterStatus;
    public ECharacterStatus CharacterStatus { get; private set; }

    public void ChangeCharacterStatus(ECharacterStatus newCharacterStatus)
    {
        if (newCharacterStatus != CharacterStatus)
        {
            _lastCharacterStatus = CharacterStatus;
            CharacterStatus = newCharacterStatus;
        }
    }
}
