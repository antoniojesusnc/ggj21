using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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

    private bool _firstCollectable;
    
    void Start()
    {
        _levelController = GetComponent<LevelController>();
        
        RandomizeCollectables();
        GetAllCollectables();
    }

    private void RandomizeCollectables()
    {
        var allCollectables = FindObjectsOfType<CollectableController>();
        List<CollectableController> shuffleCollectables = new List<CollectableController>();
        shuffleCollectables.AddRange(allCollectables);
        shuffleCollectables = Shuffle(shuffleCollectables);

        for (int i = shuffleCollectables.Count - 1; i >= WorldController.WorldData.numCollectables; i--)
        {
            shuffleCollectables[i].gameObject.SetActive(false);
        }
    }

    public static List<T> Shuffle<T>(List<T> _list)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            T temp = _list[i];
            int randomIndex = Random.Range(i, _list.Count);
            _list[i] = _list[randomIndex];
            _list[randomIndex] = temp;
        }

        return _list;
    }

    private void GetAllCollectables()
    {
        _collectables = new Dictionary<Vector2Int, CollectableController>();
        var allCollectables = FindObjectsOfType<CollectableController>();
        for (int i = 0; i < allCollectables.Length; i++)
        {
            if (!allCollectables[i].isActiveAndEnabled)
            {
                continue;
            }
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

            if (!_firstCollectable)
            {
                MessageController.Instance.ShowMessage(EMessageType.FirstCollectable);
            }
            
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
