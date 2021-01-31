using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSetVisibility : MonoBehaviour
{
    public EPlayerVisiblity _playerVisiblity;
    
    private Vector2Int _lastPlayerPosition;
    private Vector2Int _gridPosition;
    
    private CharacterMovement _characterMovement;
    private PlayerController _playerController;
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
        _playerController = FindObjectOfType<PlayerController>();
        if (_playerController == null)
        {
            Debug.LogWarning("PlayerController not found");
            return;
        }

        _characterMovement = _playerController.GetComponent<CharacterMovement>();
        
        AdjustToGridPosition();
    }

    private void SetVisiblity()
    {
        if (_playerVisiblity == EPlayerVisiblity.Show)
        {
            _playerController.SetVisible();
        }
        if (_playerVisiblity == EPlayerVisiblity.Hide)
        {
            _playerController.SetNotVisible();
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