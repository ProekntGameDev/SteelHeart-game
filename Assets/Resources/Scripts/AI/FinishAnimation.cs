using System;
using NaughtyAttributes;
using UnityEngine;

[Serializable]
public class FinishAnimation
{
    public float Duration => _duration;
    public string PlayerState => _playerState;
    public string EnemyState => _enemyState;

    [SerializeField, AnimatorState(nameof(_playerAnimator), nameof(_playerLayer))] private string _playerState;
    [SerializeField, AnimatorState(nameof(_enemyAnimator), nameof(_enemyLayer))] private string _enemyState;

    [SerializeField] private float _duration;

    [Header("Editor Only")]

    [SerializeField] private Animator _playerAnimator;
    [SerializeField, AnimatorLayer(nameof(_playerAnimator))] private int _playerLayer;
    [SerializeField] private Animator _enemyAnimator;
    [SerializeField, AnimatorLayer(nameof(_enemyAnimator))] private int _enemyLayer;
}
