using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    private CharacterController _characterController;
    private CharacterInput _characterInput;
    private WorldController _worldController;

    public Vector2Int _gridPosition;
    
    
    // movement related
    public Vector2Int _nextGridPosition;
    private bool _moving;
    private float _movingTimeStamp;
    private Vector3 _movementOrigin;
    private Vector3 _movementDestiny;
    
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _characterInput = GetComponent<CharacterInput>();
        _worldController = GameObject.FindObjectOfType<WorldController>();

        SetInitialPosition();
    }

    [ContextMenu("SetInitialPosition")]
    private void SetInitialPosition()
    {
        transform.position = _worldController.GetWorldPosition(_gridPosition);
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
                Debug.Log("asdfasdf");
                TryToMove();
            }
        }
    }

    private void UpdateMove()
    {
        _movingTimeStamp += Time.deltaTime;
        transform.position = Vector3.Lerp(_movementOrigin, _movementDestiny,
            _movingTimeStamp/ _characterController.CharacterData._characterSpeedBySquare);

        if (IsMovementFinished())
        {
            FinishMovement();
        }
    }

    private bool IsMovementFinished()
    {
        return _movingTimeStamp >= _characterController.CharacterData._characterSpeedBySquare;
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
        if (_worldController.CanMoveTo(cellObjetive, out var cellInfo))
        {
            Debug.LogWarning($"Moving to {cellObjetive}");
            DoMovement(cellObjetive);
        }
        else
        {
            Debug.LogWarning($"CANNOT Move to {cellObjetive}");
        }
    }

    private void DoMovement(Vector2Int newCellPosition)
    {
        _nextGridPosition = newCellPosition;
        _characterController.ChangeCharacterStatus(ECharacterStatus.Moving);
        _moving = true;
        _movingTimeStamp = 0;
        _movementOrigin = _worldController.GetWorldPosition(_gridPosition);
        _movementDestiny = _worldController.GetWorldPosition(_nextGridPosition);
    }
    
    private void FinishMovement()
    {
        _characterController.ChangeCharacterStatus(ECharacterStatus.Idle);
        transform.position = _worldController.GetWorldPosition(_nextGridPosition);
        _gridPosition = _nextGridPosition;
        _moving = false;
    }
    
    private bool IsMoving()
    {
        return _moving;
    }
    
    private Vector2Int GetCellObjetive()
    {
        Vector2Int cellObjetive = _gridPosition;
        switch (_characterInput.CurrentStatus)
        {
            case ECharacterMovementStatus.UP:
                return cellObjetive + Vector2Int.up;
            case ECharacterMovementStatus.DOWN:
                return cellObjetive + Vector2Int.down;
            case ECharacterMovementStatus.LEFT:
                return cellObjetive + Vector2Int.left;
            case ECharacterMovementStatus.RIGHT:
                return cellObjetive + Vector2Int.right;
        }

        return cellObjetive;
    }
}
