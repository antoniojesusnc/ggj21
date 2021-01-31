using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSetVisibility : MonoBehaviour
{
    public EPlayerVisiblity _playerVisiblity;
    
    private Vector2Int _lastPlayerPosition;
    private Vector2Int _gridPosition;
    
    private CharacterMovement _characterMovement;
    private PlayerController _characterController;
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
    
    void Start()
    {
        _characterController = FindObjectOfType<PlayerController>();
        _characterMovement = FindObjectOfType<CharacterMovement>(); 
        
        AdjustToGridPosition();
    }

    private void SetVisiblity()
    {
        if (_playerVisiblity == EPlayerVisiblity.Show)
        {
            _characterController.SetVisible();
        }
        if (_playerVisiblity == EPlayerVisiblity.Hide)
        {
            _characterController.SetNotVisible();
        }
    }
    [ContextMenu("AdjustToGridPosition")]
    private void AdjustToGridPosition()
    {
        _gridPosition = WorldController.GetGridPosition(transform.position);
        transform.position = WorldController.GetWorldPosition(_gridPosition);
    }

    void Update()
    {
        if (HasPlayerNewPos())
        {
            if (_characterMovement.GridPosition == _gridPosition)
            {
                SetVisiblity();
            }
        }
    }

    private bool HasPlayerNewPos()
    {
        if (_lastPlayerPosition != _characterMovement.GridPosition)
        {
            _lastPlayerPosition = _characterMovement.GridPosition;
            return true;
        }

        return false;
    }
}

public enum EPlayerVisiblity
{
    Show,
    Hide,
}