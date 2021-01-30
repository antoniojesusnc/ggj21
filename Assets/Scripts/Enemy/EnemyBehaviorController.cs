using UnityEngine;

public class EnemyBehaviorController : MonoBehaviour
{
    private EnemyController _enemyController;

    public ECharacterInput _temp;
    public int _input;
    public int _debugInput;
    public float _timestamp;
    public float _walkFor;
    void Start()
    {
        _enemyController = GetComponent<EnemyController>();
    }
    
    void Update()
    {
        _timestamp += Time.deltaTime;
        if (_timestamp > _walkFor)
        {
            _input = (_debugInput++) % 5;
            _timestamp = 0;
        }
    }

    public ECharacterInput GetInput()
    {
        _temp = (ECharacterInput)_input;
        return _temp;
    }
}
