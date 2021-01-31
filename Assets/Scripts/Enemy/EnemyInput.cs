using System.Collections.Generic;
using UnityEngine;

public class EnemyInput : MonoBehaviour, ICharacterInput
{
    public ECharacterInput CurrentInput { get; private set; }
    
    private EnemyBehaviorController _enemyBehaviorController;

    [SerializeField] private MovementType _movementType;
    [SerializeField]
    private List<Transform> _wayPoints;
    private List<Vector2Int> _wayPointsVector;
    private int _wayPointIndex;
    
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

    
    public bool HasMovement => CurrentInput != ECharacterInput.NONE;

    void Start()
    {
        _enemyBehaviorController = GetComponent<EnemyBehaviorController>();

        GetGridPath();
        
    }

    private void GetGridPath()
    {
        _wayPointsVector = new List<Vector2Int>();
        for (int i = 0; i < _wayPoints.Count; i++)
        {
            var gridPosition = WorldController.GetGridPosition(_wayPoints[i].transform.position);
            _wayPointsVector.Add(gridPosition);
        }
    }

    void Update()
    {
        CurrentInput = _enemyBehaviorController.GetInput();
    }
}

public enum MovementType
{
    PingPong,
    Loop,
}
