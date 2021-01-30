using UnityEngine;

public class PlayerController : CharacterController
{
    private LevelController _levelController;

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
            _levelController.CheckForCollectable(this);
            _levelController.CheckForSafeArea(this);
        }
    }

    public bool WasMoving => LastCharacterStatus == ECharacterStatus.Moving &&
                             CharacterStatus == ECharacterStatus.Idle;

}
