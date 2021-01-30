using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public const string LAYER_WALL = "Wall";
    public const string LAYER_OBSTACLE = "Obstacle";

    [SerializeField] private WorldData _worldData;

    [Header("Fist square to start calculations")] [SerializeField]
    private GameObject _botLeftSquare;

    public LayerMask _colisionLayers;

    [SerializeField] private Dictionary<Vector2Int, CellInfo> _grid;

    [Header("DEBUG GRID SCENE")] [SerializeField]
    private bool _showGridScene;
    
    private void Awake()
    {
        GenerateGrid();
    }


    [ContextMenu("Generate Grid")]
    private void GenerateGrid()
    {
        _grid = new Dictionary<Vector2Int, CellInfo>();

        CheckWallsHorizontal();
        CheckWallsVertical();
    }

    private void CheckWallsVertical()
    {
        var wallDetector = _botLeftSquare.transform.position + _botLeftSquare.transform.up * 1;
        for (int i = 0; i < _worldData.GridSize.y; i++)
        {
            Ray ray = new Ray(wallDetector, _botLeftSquare.transform.right);
            var allWalls = Physics.RaycastAll(ray, _worldData.GridSize.y * _worldData.CellSize, _colisionLayers);

            for (int j = 0; j < allWalls.Length; j++)
            {
                int wallXCell = Mathf.RoundToInt(allWalls[j].distance);
                AddCellInto(new Vector2Int(wallXCell, i), GetECellTypeFromLayer(allWalls[j]));
            }

            wallDetector += _botLeftSquare.transform.forward * _worldData.CellSize;
        }
    }

    private void CheckWallsHorizontal()
    {
        var wallDetector = _botLeftSquare.transform.position + _botLeftSquare.transform.up * 1;
        for (int i = 0; i < _worldData.GridSize.x; i++)
        {
            Ray ray = new Ray(wallDetector, _botLeftSquare.transform.forward);
            var allWalls = Physics.RaycastAll(ray, _worldData.GridSize.y * _worldData.CellSize, _colisionLayers);

            for (int j = 0; j < allWalls.Length; j++)
            {
                int wallYCell = Mathf.RoundToInt(allWalls[j].distance);
                AddCellInto(new Vector2Int(i, wallYCell), GetECellTypeFromLayer(allWalls[j]));
            }

            wallDetector += _botLeftSquare.transform.right * _worldData.CellSize;
        }
    }

    private ECellType GetECellTypeFromLayer(RaycastHit allWall)
    {
        if (allWall.transform.gameObject.layer == LayerMask.NameToLayer(LAYER_WALL))
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
        //Debug.Log($"wall detect {cellPosition}");
        /*
        var tempGo = new GameObject(cellPosition.ToString());
        tempGo.transform.position = GetWorldPosition(cellPosition);
            */
        if (!_grid.TryGetValue(cellPosition, out var cellInfo))
        {
            _grid.Add(cellPosition, new CellInfo(cellPosition, type));
        }
        else
        {
            //Debug.LogWarning($"Grid cell Info already exist in {cellPosition.x}{cellPosition.y}");
        }
    }

    public bool CanMoveTo(Vector2Int cellObjetive, out CellInfo cellInfo)
    {
        if (IsOutOfBounds(cellObjetive))
        {
            cellInfo = null;
            return false;
        }

        if (_grid.TryGetValue(cellObjetive, out cellInfo))
        {
            return cellInfo.IsWalkable();
        }

        return true;
    }

    private bool IsOutOfBounds(Vector2Int cellObjetive)
    {
        if (cellObjetive.x < 0 || cellObjetive.y < 0)
        {
            return true;
        }

        if (cellObjetive.x >= _worldData.GridSize.x ||
            cellObjetive.y >= _worldData.GridSize.y)
        {
            return true;
        }

        return false;
    }

    public Vector3 GetWorldPosition(Vector2Int nextGridPosition)
    {
        Vector3 positionIncrement = new Vector3(
            _worldData.CellSize * nextGridPosition.x,
            0,
            _worldData.CellSize * nextGridPosition.y
        );
        
        Vector3 worldPosition = _botLeftSquare.transform.position + positionIncrement;
        worldPosition.y = _worldData.ExtraHeight;
        return worldPosition;
    }

    private void OnDrawGizmos()
    {
        if (!_showGridScene)
        {
            return;
        }

        
        // draw horizontal
        var lineOrigin  = _botLeftSquare.transform.position + _botLeftSquare.transform.up * 0.1f;
        lineOrigin -= _botLeftSquare.transform.right * _worldData.CellSize * 0.5f;
        lineOrigin -= _botLeftSquare.transform.forward * _worldData.CellSize * 0.5f;
        var lineDestiny = lineOrigin + _botLeftSquare.transform.right * _worldData.CellSize * _worldData.GridSize.x;
        Vector3 step = _botLeftSquare.transform.forward * _worldData.CellSize;
        for (int i = 0; i < _worldData.GridSize.y; i++)
        {
            Debug.DrawLine(lineOrigin, lineDestiny, Color.blue);
            lineOrigin += step;
            lineDestiny += step;
        }
        
        // draw vertical
        lineOrigin  = _botLeftSquare.transform.position + _botLeftSquare.transform.up * 0.1f;
        lineOrigin -= _botLeftSquare.transform.right * _worldData.CellSize * 0.5f;
        lineOrigin -= _botLeftSquare.transform.forward * _worldData.CellSize * 0.5f;
        lineDestiny = lineOrigin + _botLeftSquare.transform.forward * _worldData.CellSize * _worldData.GridSize.y;
        step = _botLeftSquare.transform.right * _worldData.CellSize;
        for (int i = 0; i < _worldData.GridSize.x; i++)
        {
            Debug.DrawLine(lineOrigin, lineDestiny, Color.blue);
            lineOrigin += step;
            lineDestiny += step;
        }
    }
}
