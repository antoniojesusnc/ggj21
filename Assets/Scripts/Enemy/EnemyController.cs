using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class EnemyController : CharacterController
{
    public FieldOfViewData FieldOfViewData => _fieldOfViewData;
    [SerializeField] private FieldOfViewData _fieldOfViewData;

    private FieldOfViewController _fieldOfViewController;
    private bool _playerDetected;
    private float _timeStamp;

    private Vector3 _lastDirectionPlayerDetected;

    private void Start()
    {
        _fieldOfViewController = GetComponentInChildren<FieldOfViewController>();
    }

    private void LateUpdate()
    {
        _fieldOfViewController.SetLookDirection(LookDirection);

        if (_playerDetected)
        {
            UpdatePlayerDetection();
        }
    }

    private void UpdatePlayerDetection()
    {
        _timeStamp += Time.deltaTime;
        if (_timeStamp >= _fieldOfViewData.timeFreezeWhenPlayerDetected)
        {
            FinishPlayerDetection();
        }
    }

    private void FinishPlayerDetection()
    {
        _playerDetected = false;
        ChangeCharacterStatus(ECharacterStatus.Idle);
        _fieldOfViewController.ResetLookDirection();
    }

    public void CharacterDetected(Vector3 directionToTarget)
    {
        _lastDirectionPlayerDetected = directionToTarget;
        if (CharacterStatus != ECharacterStatus.PlayerDetected)
        {
            ChangeCharacterStatus(ECharacterStatus.PlayerDetected);
        }
        else
        {
            LookToDirection(_lastDirectionPlayerDetected);
        }
    }

    public override void FinishMovement()
    {
        if (CharacterStatus == ECharacterStatus.PlayerDetected)
        {
            LookToDirection(_lastDirectionPlayerDetected);
        }
        else
        {
            ChangeCharacterStatus(ECharacterStatus.Idle);
        }
    }

    private void LookToDirection(Vector3 directionToTarget)
    {
        _playerDetected = true;
        _timeStamp = 0;

        Vector3 lookObjetive = _fieldOfViewController.transform.position + directionToTarget;
        _fieldOfViewController.transform.LookAt(lookObjetive);

        CheckIfNecesaryFlip(directionToTarget);
    }

    private void CheckIfNecesaryFlip(Vector3 directionToTarget)
    {
        /*
        var characterMovement = GetComponent<CharacterMovement>();

        Vector2Int offset = new Vector2Int(
            Mathf.RoundToInt(directionToTarget.x),
            Mathf.RoundToInt(directionToTarget.z)
        );
        var movementDir = characterMovement.GetMovementDirStopped(offset);
        characterMovement.SetAnimation(movementDir);
        */
    }
}