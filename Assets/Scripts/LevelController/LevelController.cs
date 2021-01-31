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

    public bool IsLevelFinished { get; private set; }
    
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
        bool collected = _collectableManager.TryCollectFromPosition(gridPosition, out var collectableType);
        if (collected)
        {
            var uiSillouette = FindObjectOfType<UISillouettesContoller>();
            if (uiSillouette == null)
            {
                Debug.LogWarning("UISillouettesContoller not found");
                return;
            }
            
            uiSillouette.SetSillouteAsComplete(collectableType);
        }
    }

    public void CheckForSafeArea(PlayerController playerController)
    {
        _safeCellManager.CheckForSafeArea(playerController.GridPosition);
    }

    void Update()
    {
        UIGame.Instance.SetBarValue(_detectionController.Value);
    }

    public void AllTilesCollected()
    {
        MessageController.Instance.ShowMessage(EMessageType.AllCollectable);
    }

    public void WinLevel()
    {
        MessageController.Instance.ShowMessage(EMessageType.LevelComplete);

        DisableAllElementsBecauseLevelComplete();
        IsLevelFinished = true;
        
        AudioController.Instance.StopAllSounds();
        UIGame.Instance.OpenVictoryPopup();
    }
    
    public void GameOverLevel()
    {
        DisableAllElementsBecauseLevelComplete();
        IsLevelFinished = true;
        
        AudioController.Instance.StopAllSounds();
        UIGame.Instance.OpenGameOver();
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
