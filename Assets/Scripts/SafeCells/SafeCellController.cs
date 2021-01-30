using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeCellController : MonoBehaviour
{
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
    
    public Vector2Int GridPosition;
    
    void Start()
    {
        CalculateGridPosition();
    }
    
    [ContextMenu("CalculateGridPosition")]
    private void CalculateGridPosition()
    {
        GridPosition = WorldController.GetGridPosition(transform.position);
        AdjustToGridPosition();
    }

    private void AdjustToGridPosition()
    {
        transform.position = WorldController.GetWorldPosition(GridPosition);
        
        WorldController.AddCellInto(GridPosition, ECellType.SafeArea);
    }
}
