using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableMovement : MonoBehaviour
{
    private CollectableController _collectableController;
    private Vector3 _originalPosition;
    private float _totalDeltaTime;
    
    void Start()
    {
        _collectableController = GetComponent<CollectableController>();
        _originalPosition = transform.position;
    }

    void Update()
    {
        UpdateFloatingEffect();
    }

    private void UpdateFloatingEffect()
    {
        _totalDeltaTime += Time.deltaTime;

        Vector3 newHeight = _originalPosition +
                            Vector3.up *
                            Mathf.Sin(_totalDeltaTime/_collectableController.CollectableData.floatingDuration) *
                            _collectableController.CollectableData.heightIncrement;
        transform.position = newHeight;
    }
}
