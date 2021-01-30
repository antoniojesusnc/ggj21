using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class LevelController : MonoBehaviour
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

    private Dictionary<Vector2Int, CollectableController> Collectables => _collectables;
    private Dictionary<Vector2Int, CollectableController> _collectables; 
    
    void Start()
    {
        GetAllCollectables();        
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

    public void CharacterDetected(float distanceToTarget)
    {
        Debug.Log($"CharacterDetected at {distanceToTarget}m");
    }

    public void CheckForCollectable(PlayerController playerController)
    {
        var characterMovement = playerController.GetComponent<CharacterMovement>();
        if (characterMovement == null)
        {
            return;
        }

        var cellInfo = WorldController.GetCellInfo(characterMovement.GridPosition);

        if (cellInfo.IsCollectable)
        {
            CollectFromPosition(characterMovement.GridPosition);
        }
    }

    private void CollectFromPosition(Vector2Int gridPosition)
    {
        if(_collectables.TryGetValue(gridPosition, out var collectableController))
        {
            collectableController.ChangeCollectableStatus(ECollectableStatus.Collected);
            WorldController.AddCellInto(gridPosition, ECellType.None);

            CheckIfAllCollected();
        }
        else
        {
            Debug.LogError($"No collectable for position {gridPosition}");
        }
    }

    private void CheckIfAllCollected()
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

        if (allCollected)
        {
            Debug.Log("All Collected!! RUN!!");
        }
    }
}
