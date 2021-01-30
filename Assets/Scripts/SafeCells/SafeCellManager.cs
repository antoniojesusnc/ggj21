using System.Collections.Generic;

using UnityEngine;

public class SafeCellManager : MonoBehaviour
{
    private LevelController _levelController;
    
    private Dictionary<Vector2Int, SafeCellController> SafeCells => _safeCells;
    private Dictionary<Vector2Int, SafeCellController> _safeCells; 
    
    void Start()
    {
        GetAllSafeCells();
        _levelController.GetComponent<LevelController>();
    }

    private void GetAllSafeCells()
    {
        _safeCells = new Dictionary<Vector2Int, SafeCellController>();
        var allSafeCells = FindObjectsOfType<SafeCellController>();
        for (int i = 0; i < allSafeCells.Length; i++)
        {
            _safeCells.Add(allSafeCells[i].GridPosition, allSafeCells[i]);
        }
    }

    public void CheckForSafeArea(Vector2Int gridPosition)
    {
        if (_safeCells.TryGetValue(gridPosition, out var safeCellController))
        {
            _levelController.WinLevel();
        }
    }
}
