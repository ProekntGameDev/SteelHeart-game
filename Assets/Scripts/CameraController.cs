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
        [SerializeField] private float _zoomStep;
        private float _zoomDuration = 0.5f;
        private Camera _camera;
        private float _defaultFov;

        private void Start()
        {
            Application.targetFrameRate = 144;
        }
        private void Awake()
        {
            _camera = Camera.main;
            
            if(_camera)
                _defaultFov = _camera.fieldOfView;
        }

        private void LateUpdate()
        {
            if (_target) transform.position = _target.position - _offset;
        }

        public async Task ZoomIn()
        {
            while (_camera.fieldOfView > _defaultFov)
            {
                _camera.fieldOfView -= _zoomStep;
                await UniTask.NextFrame();
            }

            await Task.CompletedTask;
        }

        public async Task ZoomOut()
        {
            float targetFov = 40f;
            while (_camera.fieldOfView < targetFov)
            {
                _camera.fieldOfView += _zoomStep;
                await UniTask.NextFrame();
            }

            await Task.CompletedTask;
        }

        public async void Shake(float duration, float magnitudeX = 0.1f, float magnitudeY = 0.1f)
        {
            var pos = transform.position;

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
    }
}