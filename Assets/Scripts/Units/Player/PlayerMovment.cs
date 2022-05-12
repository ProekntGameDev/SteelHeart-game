using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SteelHeart
{
    public class PlayerMovment : MonoBehaviour
    {
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float boostSpeed = 15f;
        private Rigidbody _rigidbody;
        public bool IsGrounded;

        [FormerlySerializedAs("BoostImpulse")] public int BoostImpulse = 5000;
        private float _directionX;

        // [Header(" ")] public Image HpBar;
        // public Text CountHpBar;
        //
        // [Header("Dead screen")] public GameObject CanvasDead;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            // CanvasDead.gameObject.SetActive(false);
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        private void OnCollisionEnter()
        {
            IsGrounded = true;
        }

        private void OnCollisionExit()
        {
            IsGrounded = false;
        }

        public void Move()
        {
            var horizontalAxis = Input.GetAxis("Horizontal");
            _rigidbody.velocity = new Vector3(horizontalAxis * _speed, _rigidbody.velocity.y, 0);

            if (_rigidbody.velocity.x < -.01)
                transform.eulerAngles = new Vector3(0, 180, 0);
            else if (_rigidbody.velocity.x > .01)
                transform.eulerAngles = new Vector3(0, 0, 0);
        }

        public void Boost()
        {
            _speed = Input.GetKey(KeyCode.LeftShift) ? boostSpeed : _speed;

            _directionX = Input.GetAxis("Horizontal") * _speed;
        }
    }
}