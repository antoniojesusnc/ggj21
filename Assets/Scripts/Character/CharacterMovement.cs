using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    private CharacterController _characterController;
    private ICharacterInput _characterInput;

    [SerializeField]
    private EAudioType _movementSound;
    
    private WorldController WorldController
    {
        get
        {
            if (_worldController == null)
            {
                _worldController = FindObjectOfType<WorldController>();
            }

            return _worldController;
        }
    }

    private WorldController _worldController;
    private Animator _animator;

    public Vector2Int GridPosition => _gridPosition;
    private Vector2Int _gridPosition;

    private SpriteRenderer _spriteRenderer;
    private ECharacterMovement _movementDir;

    // movement related
    private Vector2Int _lastGridPosition;
    private Vector2Int _nextGridPosition;
    private bool _moving;
    private float _movingTimeStamp;
    private Vector3 _movementOrigin;
    private Vector3 _movementDestiny;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _characterInput = GetComponent(typeof(ICharacterInput)) as ICharacterInput;
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        CalculateGridPosition();
    }

    [ContextMenu("CalculateGridPosition")]
    private void CalculateGridPosition()
    {
        _gridPosition = WorldController.GetGridPosition(transform.position);
        AdjustToGridPosition();
    }

    private void AdjustToGridPosition()
    {
        transform.position = WorldController.GetWorldPosition(_gridPosition);
    }

    private void Update()
    {
        if (IsMoving())
        {
            UpdateMove();
        }
        else
        {
            if (CanMove() && _characterInput.HasMovement)
            {
                TryToMove();
            }
        }
    }

    private void UpdateMove()
    {
        _movingTimeStamp += Time.deltaTime;

        float delayTime = _characterController.CharacterData._characterDelayStartWalk;
        if (_movingTimeStamp < delayTime)
        {
            return;
        }

        transform.position = Vector3.Lerp(_movementOrigin, _movementDestiny,
            (_movingTimeStamp - delayTime) / _characterController.CharacterData._characterSpeedBySquare);

        AudioController.Instance.PlaySound(_movementSound);

        if (_movingTimeStamp >= _characterController.CharacterData._characterSpeedBySquare + delayTime)
        {
            FinishMovement();
        }
    }

    private bool CanMove()
    {
        switch (_characterController.CharacterStatus)
        {
            case ECharacterStatus.Idle: return true;
            default: return false;
        }
    }

    private void TryToMove()
    {
        Vector2Int cellObjetive = GetCellObjetive();
        if (WorldController.CanMoveTo(cellObjetive, out var cellInfo))
        {
            //Debug.LogWarning($"Moving to {cellObjetive}");
            StartMovement(cellObjetive);
        }
        else
        {
            if (cellInfo.Type == ECellType.Wall)
            {
                AudioController.Instance.PlaySound(EAudioType.SFXWall);
            }
        }
    }

    private void StartMovement(Vector2Int newCellPosition)
    {
        _nextGridPosition = newCellPosition;
        _characterController.ChangeCharacterStatus(ECharacterStatus.Moving);
        _moving = true;
        _movingTimeStamp = 0;
        _movementOrigin = WorldController.GetWorldPosition(_gridPosition);
        _movementDestiny = WorldController.GetWorldPosition(_nextGridPosition);

        Vector2Int offset = _nextGridPosition - _gridPosition;
        SetMovementDir(offset);
        SetLookingDir(offset);
        _animator.SetTrigger(_movementDir.ToString());
    }

    private void SetLookingDir(Vector2Int offset)
    {
        _characterController.SetLookDirection(offset);
    }

    private void SetMovementDir(Vector2Int offset)
    {
        if (offset.x > 0 || offset.y > 0)
        {
            if (_moving)
            {
                _movementDir = ECharacterMovement.BackWalk;
            }
            else
            {
                _movementDir = ECharacterMovement.BackIdle;
            }

            _spriteRenderer.flipX = offset.x > 0;
        }
        else
        {
            if (_moving)
            {
                _movementDir = ECharacterMovement.FrontWalk;
            }
            else
            {
                _movementDir = ECharacterMovement.FrontIdle;
            }

            _spriteRenderer.flipX = offset.x < 0;
        }

    }

    private void FinishMovement()
    {
        _lastGridPosition = _gridPosition;
        _gridPosition = _nextGridPosition;
        AdjustToGridPosition();
        _moving = false;
        _characterController.ChangeCharacterStatus(ECharacterStatus.Idle);

        Vector2Int offset = _gridPosition - _lastGridPosition;
        SetMovementDir(offset);
        _animator.SetTrigger(_movementDir.ToString());
    }

    private bool IsMoving()
    {
        return _moving;
    }

    private Vector2Int GetCellObjetive()
    {
        Vector2Int cellObjetive = _gridPosition;
        switch (_characterInput.CurrentInput)
        {
            case ECharacterInput.UP:
                return cellObjetive + Vector2Int.up;
            case ECharacterInput.DOWN:
                return cellObjetive + Vector2Int.down;
            case ECharacterInput.LEFT:
                return cellObjetive + Vector2Int.left;
            case ECharacterInput.RIGHT:
                return cellObjetive + Vector2Int.right;
        }

        return cellObjetive;
    }
}