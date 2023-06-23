using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NewPlayerController
{
    [RequireComponent(typeof(WalkPlayerBehaviour))]
    [RequireComponent(typeof(RunPlayerBehaviour))]
    [RequireComponent(typeof(JumpPlayerBehaviour))]
    [RequireComponent(typeof(FallPlayerBehaviour))]
    [RequireComponent(typeof(IdlePlayerBehaviour))]
    [RequireComponent(typeof(HalfSquatPlayerBehaviour))]
    public class PlayerBehaviourController : MonoBehaviour
    {
        public IPlayerBehaviour CurrentPlayerBehaviour => _currentPlayerBehaviour;

        private Dictionary<Type, IPlayerBehaviour> _playerBehaviours = new Dictionary<Type, IPlayerBehaviour>(); //dictionary of behaviors
        private IPlayerBehaviour _currentPlayerBehaviour; //current player behavior

        [SerializeField] private EnumPlayerBehaviour _playerBehaviour = EnumPlayerBehaviour.Idle; //enum of player behaviors

        private void Start()
        {
            InitPlayerBehaviours(); //init player behaviors
            SetPlayerBehaviourByDefault(); //set basic behavior
        }

        private void Update()
        {
            if (_currentPlayerBehaviour != null) _currentPlayerBehaviour.UpdateBehaviour(); //update of current behavior
        }

        private void InitPlayerBehaviours()
        {
            _playerBehaviours[typeof(WalkPlayerBehaviour)] = GetComponent<WalkPlayerBehaviour>();
            _playerBehaviours[typeof(RunPlayerBehaviour)] = GetComponent<RunPlayerBehaviour>();
            _playerBehaviours[typeof(JumpPlayerBehaviour)] = GetComponent<JumpPlayerBehaviour>();
            _playerBehaviours[typeof(FallPlayerBehaviour)] = GetComponent<FallPlayerBehaviour>();
            _playerBehaviours[typeof(IdlePlayerBehaviour)] = GetComponent<IdlePlayerBehaviour>();
            _playerBehaviours[typeof(HalfSquatPlayerBehaviour)] = GetComponent<HalfSquatPlayerBehaviour>();
        }

        private IPlayerBehaviour GetPlayerBehaviour<T>() where T : IPlayerBehaviour
        {
            var type = typeof(T);
            return _playerBehaviours[type];
        }

        private void SetCurrentPlayerBehaviour(IPlayerBehaviour newPlayerBehaviour)
        {
            if (_currentPlayerBehaviour != null) _currentPlayerBehaviour.ExitBehaviour(); //exiting the current behavior
            _currentPlayerBehaviour = newPlayerBehaviour; //setting a new current behavior
            _currentPlayerBehaviour.EnterBehaviour(); //entering the new current behavior
        }

        private void SetPlayerBehaviourByDefault()
        {
            SetIdlePlayerBehaviour(); //setting behavior Idle
        }

        public void SetWalkPlayerBehaviour()
        {
            IPlayerBehaviour behaviour = GetPlayerBehaviour<WalkPlayerBehaviour>(); //pulling out of the dictionary behavior Walk
            if (behaviour != _currentPlayerBehaviour) SetCurrentPlayerBehaviour(behaviour); //

            _playerBehaviour = EnumPlayerBehaviour.Walk;
        }

        public void SetRunPlayerBehaviour()
        {
            IPlayerBehaviour behaviour = GetPlayerBehaviour<RunPlayerBehaviour>(); //pulling out of the dictionary behavior Run
            if (behaviour != _currentPlayerBehaviour) SetCurrentPlayerBehaviour(behaviour); //setting a new current behavior

            _playerBehaviour = EnumPlayerBehaviour.Run;
        }

        public void SetJumpPlayerBehaviour()
        {
            IPlayerBehaviour behaviour = GetPlayerBehaviour<JumpPlayerBehaviour>(); //pulling out of the dictionary behavior Jump
            if (behaviour != _currentPlayerBehaviour) SetCurrentPlayerBehaviour(behaviour); //setting a new current behavior

            _playerBehaviour = EnumPlayerBehaviour.Jump;
        }

        public void SetFallPlayerBehaviour()
        {
            IPlayerBehaviour behaviour = GetPlayerBehaviour<FallPlayerBehaviour>(); //pulling out of the dictionary behavior Fall
            if (behaviour != _currentPlayerBehaviour) SetCurrentPlayerBehaviour(behaviour); //setting a new current behavior

            _playerBehaviour = EnumPlayerBehaviour.Fall;
        }

        public void SetIdlePlayerBehaviour()
        {
            IPlayerBehaviour behaviour = GetPlayerBehaviour<IdlePlayerBehaviour>(); //pulling out of the dictionary behavior Idle
            if (behaviour != _currentPlayerBehaviour) SetCurrentPlayerBehaviour(behaviour); //setting a new current behavior

            _playerBehaviour = EnumPlayerBehaviour.Idle;
        }

        public void SetHalfSquatPlayerBehaviour()
        {
            IPlayerBehaviour behaviour = GetPlayerBehaviour<HalfSquatPlayerBehaviour>(); //pulling out of the dictionary behavior HalfSquat
            if (behaviour != _currentPlayerBehaviour) SetCurrentPlayerBehaviour(behaviour); //setting a new current behavior

            _playerBehaviour = EnumPlayerBehaviour.HalfSquat;
        }
    }
}
