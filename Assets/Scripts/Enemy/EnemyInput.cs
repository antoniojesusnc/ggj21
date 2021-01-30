using System.Collections.Generic;
using UnityEngine;

public class EnemyInput : MonoBehaviour, ICharacterInput
{
    public ECharacterInput CurrentInput { get; private set; }
    
    private EnemyBehaviorController _enemyBehaviorController;

    [SerializeField] private MovementType _loop;
    [SerializeField]
    private List<Transform> WayPoints;
    public bool HasMovement => CurrentInput != ECharacterInput.NONE;

    void Start()
    {
        _enemyBehaviorController = GetComponent<EnemyBehaviorController>();
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
