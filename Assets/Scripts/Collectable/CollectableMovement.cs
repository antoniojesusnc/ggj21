using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableMovement : MonoBehaviour
{
    private CollectableController _collectableController;
    private Vector3 _originalPosition;
    private float _totalDeltaTime;
    
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

    public Vector2Int GridPosition { get; private set; }

    private WorldController _worldController;

    void Awake()
    {
        _collectableController = GetComponent<CollectableController>();
        _originalPosition = transform.position;

        AdjustToGridPosition();
    }

    [ContextMenu("AdjustToGridPosition")]
    private void AdjustToGridPosition()
    {
        GridPosition = WorldController.GetGridPosition(transform.position);
        transform.position = WorldController.GetWorldPosition(GridPosition);
        _worldController.AddCellInto(GridPosition, ECellType.Collectable);
    }
    void Update()
    {
        UpdateFloatingEffect();
    }

    private void UpdateFloatingEffect()
    {
        _totalDeltaTime += Time.deltaTime;

        Vector3 newHeight = _originalPosition +
                            Mathf.Sin(_totalDeltaTime / _collectableController.CollectableData.floatingDuration) *
                            _collectableController.CollectableData.heightIncrement * Vector3.up;
        transform.position = newHeight;
    }
}
