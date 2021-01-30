using System.Collections.Generic;
using UnityEngine;

public class CollectableManager : MonoBehaviour
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
    
    private LevelController _levelController;

    public Dictionary<Vector2Int, CollectableController> Collectables => _collectables;
    private Dictionary<Vector2Int, CollectableController> _collectables; 
    
    void Start()
    {
        GetAllCollectables();
        _levelController = GetComponent<LevelController>();
    }

    private void GetAllCollectables()
    {
        _collectables = new Dictionary<Vector2Int, CollectableController>();
        var allCollectables = FindObjectsOfType<CollectableController>();
        for (int i = 0; i < allCollectables.Length; i++)
        {
            var collectableMovement = allCollectables[i].GetComponent<CollectableMovement>();
            if (collectableMovement == null)
            {
                Debug.LogWarning($"Collectable: {allCollectables[i]} doesn't have collectable movement" );
                continue;
            }
            
            _collectables.Add(collectableMovement.GridPosition, allCollectables[i]);
        }
    }

    public bool TryCollectFromPosition(Vector2Int gridPosition, out ECollectableType collectableType)
    {
        if(_collectables.TryGetValue(gridPosition, out var collectableController))
        {
            collectableType = collectableController.CollectableType;
            collectableController.ChangeCollectableStatus(ECollectableStatus.Collected);
            WorldController.AddCellInto(gridPosition, ECellType.None);
            
            AudioController.Instance.PlaySound(EAudioType.SFXSteal);
            
            if (HasAllCollected())
            {
                _levelController.AllTilesCollected();
            }

            return true;
        }
        else
        {
            collectableType = ECollectableType.None;
            Debug.LogError($"No collectable for position {gridPosition}");
            return false;
        }
    }
    
    
    public bool HasAllCollected()
    {
        bool allCollected = true;
        foreach (var collectable in _collectables)
        {
            allCollected = collectable.Value.CollectableStatus == ECollectableStatus.Collected;
            if (!allCollected)
            {
                break;
            }
        }

        return allCollected;
    }
}
