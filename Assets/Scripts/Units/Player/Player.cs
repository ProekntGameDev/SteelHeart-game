using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SteelHeart
{
    public class Player : Unit
    {
        [SerializeField] private PlayerMovment _playerMovment;
        [Header("Health")] [SerializeField] private float _health;
        [SerializeField] private float _startTimeGodMode;
        [SerializeField] private CameraController _cameraController;
        private float _timeGodMode;

        private void Awake()
        {
            
        }

        private void Update()
        {
            // _playerMovment.Boost();
        }
        
        private void FixedUpdate()
        {
            _playerMovment.Move();

        }
        
        public async void TakeDamage(float damage)
        {
            if (_timeGodMode <= 0)
            {
                _health -= damage;

                // HpBar.fillAmount = _health / 100;
                // CountHpBar.text = "" + _health;

                _timeGodMode = _startTimeGodMode;
            }

            if (_health <= 0)
            {
                //Destroy(gameObject.GetComponent<PlayerMovment>()); 

                // CanvasDead.gameObject.SetActive(true);
                Destroy(gameObject);
            }
            
            await _cameraController.ZoomOut();
            await _cameraController.ZoomIn();
        }
    }
}