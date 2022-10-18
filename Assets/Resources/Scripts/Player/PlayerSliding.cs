using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSliding : MonoBehaviour
{
    [SerializeField] private float _slideForce = 500;
    [SerializeField] private float _maxSlideTime = 1;
    [SerializeField] private float _newColliderSize;
    [SerializeField] private Vector3 _newColliderCenter;
    [SerializeField] private KeyCode _slideKey = KeyCode.LeftControl;
    [SerializeField] private UnityEvent _onStartSliding;
    [SerializeField] private UnityEvent _onStopSliding;
    [SerializeField] private CapsuleCollider _capsuleCollider;
    [SerializeField] private Rigidbody _rigidbody;
    private bool _isSliding = false;
    private float _defaultColliderSize;
    private Vector3 _defaultColliderCenter;
    private float _horizontalAxis;
    private float _slideTimer;

    private void Awake()
    {
        _defaultColliderSize = _capsuleCollider.height;
        _defaultColliderCenter = _capsuleCollider.center;
    }

    private void Update()
    {
        _horizontalAxis = Input.GetAxis("Horizontal");
        
        if (Input.GetKeyDown(_slideKey) && (_horizontalAxis > 0.1f || _horizontalAxis < -0.1f) && _isSliding == false)
        {
            StartSlide();
        }
    }

    
    private void StartSlide()
    {
        StartCoroutine(SlideCoroutine());
    }
    
    private IEnumerator SlideCoroutine()
    {
        _slideTimer = _maxSlideTime;
        _capsuleCollider.height = _newColliderSize;
        _capsuleCollider.center = _newColliderCenter;
        
        _onStartSliding?.Invoke();
        _isSliding = true;
        
        while (_isSliding)
        {
            Vector3 inputDirection = gameObject.transform.forward * _horizontalAxis +
                                     gameObject.transform.right * _horizontalAxis;

            // sliding normal
            if (_rigidbody.velocity.y > -0.1f)
            {
                _rigidbody.AddForce(inputDirection.normalized * _slideForce, ForceMode.Force);
                _slideTimer -= Time.deltaTime;

                if (_slideTimer <= 0)
                {
                    StopSlide();


                    yield break;
                }
            }

            yield return new WaitForSeconds(0.01f);
        }
    }
    
    private void StopSlide()
    {
        _capsuleCollider.height = _defaultColliderSize;
        _capsuleCollider.center = _defaultColliderCenter;
        _onStopSliding?.Invoke();
        _isSliding = false;
    }
}
