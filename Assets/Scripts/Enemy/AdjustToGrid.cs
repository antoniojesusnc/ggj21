using UnityEngine;

public class AdjustToGrid : MonoBehaviour
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

    void Start()
    {
       
        AdjustToGridPosition();
    }

    [ContextMenu("AdjustToGridPosition")]
    private void AdjustToGridPosition()
    {
        var gridPosition = WorldController.GetGridPosition(transform.position);
        transform.position = WorldController.GetWorldPosition(gridPosition);
    }
}
