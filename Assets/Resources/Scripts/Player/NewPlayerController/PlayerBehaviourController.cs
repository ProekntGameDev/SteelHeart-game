//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NewPlayerController
{
    public class PlayerBehaviourController
    {
        public IPlayerBehaviour CurrentPlayerBehaviour => _currentPlayerBehaviour;

        private Dictionary<Type, IPlayerBehaviour> _playerBehaviours = new Dictionary<Type, IPlayerBehaviour>(); //dictionary of behaviors
        private IPlayerBehaviour _currentPlayerBehaviour; //current player behavior

        private PlayerController _playerController;

        public PlayerBehaviourController(PlayerController playerController)
        {
            _playerController = playerController;
            InitPlayerBehaviours(); //init player behaviors
            SetPlayerBehaviourByDefault(); //set basic behavior
        }

        public void SetBehaviour<T>() where T : IPlayerBehaviour
        {
            IPlayerBehaviour behaviour = GetPlayerBehaviour<T>();
            if (behaviour != _currentPlayerBehaviour) SetCurrentPlayerBehaviour(behaviour);
        }

        private void SetCurrentPlayerBehaviour(IPlayerBehaviour newPlayerBehaviour)
        {
            if (_currentPlayerBehaviour != null) _currentPlayerBehaviour.ExitBehaviour(); //exiting the current behavior
            _currentPlayerBehaviour = newPlayerBehaviour; //setting a new current behavior
            _currentPlayerBehaviour.EnterBehaviour(); //entering the new current behavior
        }

        private void InitPlayerBehaviours()
        {
            _playerBehaviours[typeof(WalkPlayerBehaviour)] = new WalkPlayerBehaviour(_playerController, this, _playerController.WalkSpeed);
            _playerBehaviours[typeof(RunPlayerBehaviour)] = new RunPlayerBehaviour(_playerController, this, _playerController.RunSpeed);
            _playerBehaviours[typeof(JumpPlayerBehaviour)] = new JumpPlayerBehaviour(_playerController, this, _playerController.JumpForce);
            _playerBehaviours[typeof(FallPlayerBehaviour)] = new FallPlayerBehaviour(_playerController, this, _playerController.Gravity);
            _playerBehaviours[typeof(IdlePlayerBehaviour)] = new IdlePlayerBehaviour(_playerController, this);
            _playerBehaviours[typeof(HalfSquatPlayerBehaviour)] = new HalfSquatPlayerBehaviour(_playerController, this, _playerController.HalfSquatSpeed);
            _playerBehaviours[typeof(RisePlayerBehaviour)] = new RisePlayerBehaviour(_playerController, this, _playerController.RiseSpeed);
        }

        private IPlayerBehaviour GetPlayerBehaviour<T>() where T : IPlayerBehaviour
        {
            var type = typeof(T);
            return _playerBehaviours[type];
        }

        private void SetPlayerBehaviourByDefault()
        {
            IPlayerBehaviour behaviour = GetPlayerBehaviour<IdlePlayerBehaviour>(); //pulling out of the dictionary behavior Idle
            SetCurrentPlayerBehaviour(behaviour); //setting behavior Idle
        }
    }
}
