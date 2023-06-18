using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(WalkPlayerBehaviour))]
[RequireComponent(typeof(RunPlayerBehaviour))]
[RequireComponent(typeof(JumpPlayerBehaviour))]
[RequireComponent(typeof(FallPlayerBehaviour))]
[RequireComponent(typeof(IdlePlayerBehaviour))]
public class PlayerBehaviourController : MonoBehaviour
{
    public IPlayerBehaviour CurrentPlayerBehaviour => _currentPlayerBehaviour;

    private Dictionary<Type, IPlayerBehaviour> _playerBehaviours = new Dictionary<Type, IPlayerBehaviour>();
    private IPlayerBehaviour _currentPlayerBehaviour;

    [SerializeField] private EnumPlayerBehaviour _playerBehaviour = EnumPlayerBehaviour.Idle;

    private void Start()
    {
        InitPlayerBehaviours();
        SetPlayerBehaviourByDefault();
    }

    private void Update()
    {
        if (_currentPlayerBehaviour != null) _currentPlayerBehaviour.UpdateBehaviour();
    }

    private void InitPlayerBehaviours()
    {
        _playerBehaviours[typeof(WalkPlayerBehaviour)] = GetComponent<WalkPlayerBehaviour>();
        _playerBehaviours[typeof(RunPlayerBehaviour)] = GetComponent<RunPlayerBehaviour>();
        _playerBehaviours[typeof(JumpPlayerBehaviour)] = GetComponent<JumpPlayerBehaviour>();
        _playerBehaviours[typeof(FallPlayerBehaviour)] = GetComponent<FallPlayerBehaviour>();
        _playerBehaviours[typeof(IdlePlayerBehaviour)] = GetComponent<IdlePlayerBehaviour>();
    }

    private IPlayerBehaviour GetPlayerBehaviour<T>() where T : IPlayerBehaviour
    {
        var type = typeof(T);
        return _playerBehaviours[type];
    }

    private void SetCurrentPlayerBehaviour(IPlayerBehaviour newPlayerBehaviour)
    {
        if (_currentPlayerBehaviour != null) _currentPlayerBehaviour.ExitBehaviour();
        _currentPlayerBehaviour = newPlayerBehaviour;
        _currentPlayerBehaviour.EnterBehaviour();
    }

    private void SetPlayerBehaviourByDefault()
    {
        SetIdlePlayerBehaviour();
    }

    public void SetWalkPlayerBehaviour()
    {
        IPlayerBehaviour behaviour = GetPlayerBehaviour<WalkPlayerBehaviour>();
        if (behaviour != _currentPlayerBehaviour) SetCurrentPlayerBehaviour(behaviour);

        _playerBehaviour = EnumPlayerBehaviour.Walk;
    }

    public void SetRunPlayerBehaviour()
    {
        IPlayerBehaviour behaviour = GetPlayerBehaviour<RunPlayerBehaviour>();
        if (behaviour != _currentPlayerBehaviour) SetCurrentPlayerBehaviour(behaviour);

        _playerBehaviour = EnumPlayerBehaviour.Run;
    }

    public void SetJumpPlayerBehaviour()
    {
        IPlayerBehaviour behaviour = GetPlayerBehaviour<JumpPlayerBehaviour>();
        if (behaviour != _currentPlayerBehaviour) SetCurrentPlayerBehaviour(behaviour);

        _playerBehaviour = EnumPlayerBehaviour.Jump;
    }

    public void SetFallPlayerBehaviour()
    {
        IPlayerBehaviour behaviour = GetPlayerBehaviour<FallPlayerBehaviour>();
        if (behaviour != _currentPlayerBehaviour) SetCurrentPlayerBehaviour(behaviour);

        _playerBehaviour = EnumPlayerBehaviour.Fall;
    }

    public void SetIdlePlayerBehaviour()
    {
        IPlayerBehaviour behaviour = GetPlayerBehaviour<IdlePlayerBehaviour>();
        if (behaviour != _currentPlayerBehaviour) SetCurrentPlayerBehaviour(behaviour);

        _playerBehaviour = EnumPlayerBehaviour.Idle;
    }
}
