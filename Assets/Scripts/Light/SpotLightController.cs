using UnityEngine;

public class SpotLightController : MonoBehaviour
{
    private EnemyController _enemyController;
    private Light _light;

    void Start()
    {
        _enemyController = GetComponentInParent<EnemyController>();
        _light = GetComponent<Light>();
        
        SetLightProperties();
    }

    private void SetLightProperties()
    {
        _light.spotAngle = _enemyController.FieldOfViewData.coneAngleToShow +_enemyController.FieldOfViewData.lightExtraAngle;
        _light.range = _enemyController.FieldOfViewData.coneDistanceToShow +_enemyController.FieldOfViewData.lightExtraDistance;
    }
}