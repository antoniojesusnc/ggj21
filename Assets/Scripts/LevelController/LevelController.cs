using System.Collections.Generic;

using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static bool CanInput; 
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

    private CollectableManager _collectableManager;
    private SafeCellManager _safeCellManager;
    private DetectionController _detectionController;

    private List<MonoBehaviour> _disableWhenLevelCompleteElements;

    public void AddDisableWhenLevelComplete(MonoBehaviour componentToDisable)
    {
        if (_disableWhenLevelCompleteElements == null)
        {
            _disableWhenLevelCompleteElements = new List<MonoBehaviour>();
        }
        
        _disableWhenLevelCompleteElements.Add(componentToDisable);
    }

    private void DisableAllElementsBecauseLevelComplete()
    {
        for (int i = 0; i < _disableWhenLevelCompleteElements.Count; i++)
        {
            _disableWhenLevelCompleteElements[i].enabled = false;
        }
    }
    
    void Start()
    {
        _collectableManager = GetComponent<CollectableManager>();
        _safeCellManager =  GetComponent<SafeCellManager>();
        _detectionController = GetComponent<DetectionController>();
    }
    
    public void CharacterDetected(float distanceToTarget)
    {
        _detectionController.CharacterDetected(distanceToTarget);
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
        _collectableManager.CollectFromPosition(gridPosition);
    }

    public void CheckForSafeArea(PlayerController playerController)
    {
        _safeCellManager.CheckForSafeArea(playerController.GridPosition);
    }

    public void AllTilesCollected()
    {
        Debug.Log("RUN!!!");
    }

    public void WinLevel()
    {
        Debug.Log("Win!!!");
        DisableAllElementsBecauseLevelComplete();
    }
    
    public void GameOverLevel()
    {
        Debug.Log("Game Over !!!");
        DisableAllElementsBecauseLevelComplete();
    }

    public void PlayerMoveToNewCell(PlayerController playerController)
    {
        if (!_collectableManager.HasAllCollected())
        {
            CheckForCollectable(playerController);
        }
        else
        {
            CheckForSafeArea(playerController);
        }
    }
}
