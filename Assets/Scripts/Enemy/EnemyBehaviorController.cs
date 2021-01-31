using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorController : MonoBehaviour
{
    private EnemyController _enemyController;
    private CharacterMovement _characterMovement;

    [SerializeField] private MovementType _movementType;
    [SerializeField] private List<Transform> _wayPoints;
    private List<Vector2Int> _wayPointsVector;
    private int _wayPointIndex;

    private Vector2Int _nextWayPoint;
    private Vector2Int _nextStep;

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
        _enemyController = GetComponent<EnemyController>();
        _characterMovement = GetComponent<CharacterMovement>();
        if (_wayPoints.Count > 0)
        {
            GetGridPath();
            if (_wayPointsVector.Count > 0)
            {
                _nextWayPoint = _wayPointsVector[0];
            }
        }
    }

    private void GetGridPath()
    {
        _wayPointsVector = new List<Vector2Int>();
        for (int i = 0; i < _wayPoints.Count; i++)
        {
            var gridPosition = WorldController.GetGridPosition(_wayPoints[i].transform.position);
            _wayPointsVector.Add(gridPosition);
        }

        if (_movementType == MovementType.PingPong)
        {
            int numElements = _wayPointsVector.Count;
            for (int i = numElements - 2; i >= 1; i--)
            {
                if (numElements > 0 && numElements < _wayPointsVector.Count)
                {
                    _wayPointsVector.Add(_wayPointsVector[i]);
                }
            }
        }
    }

    void Update()
    {
        if (_wayPointsVector == null || _wayPointsVector.Count <= 0)
        {
            return;
        } 
        
        if (_enemyController.CharacterStatus == ECharacterStatus.Idle)
        {
            CheckNextStep();
        }
        else if (_enemyController.CharacterStatus == ECharacterStatus.Moving)
        {
            _nextStep = Vector2Int.zero;
        }
    }

    private void CheckNextStep()
    {
        if (_nextWayPoint == _characterMovement.GridPosition)
        {
            _nextWayPoint = GetNextWayPointIndex();
        }
        var movement = _nextWayPoint - _characterMovement.GridPosition;

        if (movement.x != 0)
        {
            _nextStep = Vector2Int.right * movement.x;
        }
        else
        {
            _nextStep = Vector2Int.up * movement.y;
        }
    }

    private Vector2Int GetNextWayPointIndex()
    {
        _wayPointIndex++;
        if (_wayPointIndex >= _wayPoints.Count)
        {
            _wayPointIndex = 0;
        }

        return _wayPointsVector[_wayPointIndex];
    }

    public ECharacterInput GetInput()
    {
        return GetInputBasedOnNextGrid();
    }

    private ECharacterInput GetInputBasedOnNextGrid()
    {
        if (_nextStep.x > 0)
        {
            return ECharacterInput.RIGHT;
        }
        else if (_nextStep.x < 0)
        {
            return ECharacterInput.LEFT;
        }

        if (_nextStep.y > 0)
        {
            return ECharacterInput.UP;
        }
        else if (_nextStep.y < 0)
        {
            return ECharacterInput.DOWN;
        }

        return ECharacterInput.NONE;
    }
}
