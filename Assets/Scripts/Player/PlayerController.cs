using UnityEngine;

public class PlayerController : CharacterController
{
    private LevelController _levelController;

    [SerializeField] private Material _viewMaterial;
    [SerializeField] private Material _hideMaterial;
    
    public Vector2Int GridPosition
    {
        get
        {
            var characterMovement = GetComponent<CharacterMovement>();
            if (characterMovement != null)
            {
                return characterMovement.GridPosition;
            }
            else
            {
                return Vector2Int.zero;
            }
        }
    }
    void Start()
    {
        _levelController = FindObjectOfType<LevelController>();
    }
    
    public override void ChangeCharacterStatus(ECharacterStatus newCharacterStatus)
    {
        base.ChangeCharacterStatus(newCharacterStatus);

        if (WasMoving)
        {
            _levelController.PlayerMoveToNewCell(this);
        }
    }

    public bool WasMoving => LastCharacterStatus == ECharacterStatus.Moving &&
                             CharacterStatus == ECharacterStatus.Idle;

    public void SetVisible()
    {
        GetComponentInChildren<SpriteRenderer>().material = _viewMaterial;
    }
    
    public void SetNotVisible()
    {
        GetComponentInChildren<SpriteRenderer>().material = _hideMaterial;
    }
}
