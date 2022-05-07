using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SteelHeart
{

    
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private Transform _zoomTarget;

        private void Awake()
        {
        }

        private void Start()
        {
            Application.targetFrameRate = 144;
            FollowToPlayer();
            // Shake(3f);
            // ZoomIn();
        }

        private void Update()
        {
        }

        private void FixedUpdate()
        {
            if (_target)
                FollowToPlayer();
        }

        private async void ZoomIn()
        {
            var pos = transform.position;
            while (transform.position.x != _zoomTarget.position.x)
            {
                pos.x += 0.1f;
                // pos.z += 0.1f;

                transform.position = pos;
                await UniTask.NextFrame();
            }
        }

        private async void Shake(float duration)
        {
            var pos = transform.position;
            float magnitudeX = 0.1f;
            float magnitudeY = 0.1f;

            while (duration > 0)
            {
                float x = Random.Range(pos.x -= magnitudeX, pos.x += magnitudeX);
                float y = Random.Range(pos.y -= magnitudeY, pos.y += magnitudeY);

                transform.position = new Vector3(x, y, pos.z);
                duration -= Time.deltaTime;
                await UniTask.NextFrame();
            }

            transform.position = pos;
        }

        private void FollowToPlayer()
        {
            transform.position = _target.position - _offset;
        }
    }
}