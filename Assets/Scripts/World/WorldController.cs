using System;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public const string LAYER_WALL = "Wall";
    public const string LAYER_OBSTACLE = "Obstacle";
    
    [SerializeField] private WorldData _worldData;
    
    [Header("Fist square to start calculations")]
    [SerializeField] private GameObject _botLeftSquare;

    public LayerMask _colisionLayers;

    [SerializeField]
    private Dictionary<Vector2Int, CellInfo> _grid;
    private void Awake()
    {
        GenerateGrid();
    }
    
    
    [ContextMenu("Generate Grid")]
    private void GenerateGrid()
    {
        
        _grid = new Dictionary<Vector2Int, CellInfo>();

        var wallDetector = _botLeftSquare.transform.position + _botLeftSquare.transform.up * 1;
        
        for (int i = 0; i < _worldData.GridSize.x; i++)
        {
            Ray ray = new Ray(wallDetector, -_botLeftSquare.transform.right);
            var allWalls = Physics.RaycastAll(ray, _worldData.GridSize.y * _worldData.CellSize, _colisionLayers);
            
            for (int j = 0; j < allWalls.Length; j++)
            {
                int wallYCell = Mathf.RoundToInt(allWalls[j].distance);
                AddCellInto(new Vector2Int(i, wallYCell), GetECellTypeFromLayer(allWalls[i]));
            }
            
            wallDetector += _botLeftSquare.transform.forward * _worldData.CellSize;
        }
    }

    private ECellType GetECellTypeFromLayer(RaycastHit allWall)
    {
        if(allWall.transform.gameObject.layer == LayerMask.NameToLayer(LAYER_WALL))
        {
            return ECellType.Wall;
        }
        else if (allWall.transform.gameObject.layer == LayerMask.NameToLayer(LAYER_OBSTACLE))
        {
            return ECellType.Obstacle;            
        }

        return ECellType.None;
    }

    private void AddCellInto(Vector2Int cellPosition, ECellType type)
    {
        if (!_grid.TryGetValue(cellPosition, out var cellInfo))
        {
            _grid.Add(cellPosition, new CellInfo(cellPosition, type));
        }
        else
        {
            Debug.LogWarning($"Grid cell Info already exist in {cellPosition.x}{cellPosition.y}");
        }
    }

    public bool CanMoveTo(Vector2Int cellObjetive, out CellInfo cellInfo)
    {
        if (_grid.TryGetValue(cellObjetive, out cellInfo))
        {
            return cellInfo.IsWalkable();
        }

        return true;
    }
}
