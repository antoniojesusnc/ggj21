using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private CharacterData CharacterData => _characterData;
    [SerializeField]
    private CharacterData _characterData;
}
