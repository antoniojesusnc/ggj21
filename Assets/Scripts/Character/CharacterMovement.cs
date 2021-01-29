using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    private CharacterController _characterController;
    private CharacterInput _characterInput;
    private WorldController _worldController;

    public Vector2Int _gridPosition;
    
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _characterInput = GetComponent<CharacterInput>();
        _worldController = GameObject.FindObjectOfType<WorldController>();
    }

    private void Update()
    {
        if (HasMovement())
        {
            TryToMove();
        }
    }

    private void TryToMove()
    {
        Vector2Int cellObjetive = GetCellObjetive();
        if (_worldController.CanMoveTo(cellObjetive, out var cellInfo))
        {
            
        }
    }

    private Vector2Int GetCellObjetive()
    {
        Vector2Int cellObjetive = _gridPosition;
        switch (_characterInput.CurrentStatus)
        {
            case CharacterMovementStatus.UP:
                return cellObjetive + Vector2Int.up;
            case CharacterMovementStatus.DOWN:
                return cellObjetive + Vector2Int.down;
            case CharacterMovementStatus.LEFT:
                return cellObjetive + Vector2Int.left;
            case CharacterMovementStatus.RIGHT:
                return cellObjetive + Vector2Int.right;
        }

        return cellObjetive;
    }

    private bool HasMovement()
    {
        return _characterInput.CurrentStatus != CharacterMovementStatus.NONE;
    }
}
