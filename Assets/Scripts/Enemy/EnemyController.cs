using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharacterController
{
    public FieldOfViewData FieldOfViewData => _fieldOfViewData;
    [SerializeField]
    private FieldOfViewData _fieldOfViewData;

    private FieldOfViewController _fieldOfViewController;

    private void Start()
    {
        _fieldOfViewController = GetComponentInChildren<FieldOfViewController>();
    }

    private void LateUpdate()
    {
        _fieldOfViewController.SetLookDirection(LookDirection);
    }
}
