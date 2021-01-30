using UnityEngine;

public class EnemyInput : MonoBehaviour, ICharacterInput
{
    public ECharacterInput CurrentInput { get; private set; }
    
    private EnemyBehaviorController _enemyBehaviorController;
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

