using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageTriggerWhenPlayerAt : MonoBehaviour
{
    [SerializeField] private EMessageType _messageType;

    private Vector2Int _lastPlayerPosition;
    private CharacterMovement _characterMovement;

    private Vector2Int _gridPosition;
    
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
        var playerController = FindObjectOfType<PlayerController>();
        if (playerController == null)
        {
            Debug.LogWarning("PlayerController not found");
            return;
        }

        _characterMovement = playerController.GetComponent<CharacterMovement>();
        _gridPosition = WorldController.GetGridPosition(transform.position);
    }

    [ContextMenu("Adjust To Grid")]
    public void AdjustToGrid()
    {
        var gridPosition = WorldController.GetGridPosition(transform.position);
        transform.position = WorldController.GetWorldPosition(gridPosition);
    }
    
    void Update()
    {
        if (HasPlayerNewPos())
        {
            if (_characterMovement.GridPosition == _gridPosition)
        {
            MessageController.Instance.ShowMessage(_messageType);
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
