using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraController : MonoBehaviour
{
    private Camera camera_component;
    private Transform target_transform;
    private Vector3 offset;
    private float zoomStep;
    private float zoomDuration = 0.5f;
    private float defaultFoV;
    //
    private float shake_duration;
    private float target_zoom;

    private void Start()
    {
        Application.targetFrameRate = 144;
    }
    private void Awake()
    {
        camera_component = gameObject.GetComponent<Camera>();
        target_zoom = defaultFoV = camera_component.fieldOfView;
    }

    private void LateUpdate()
    {
        if (target_transform) base.transform.position = target_transform.position - offset;
        if (shake_duration > 0) Shake(shake_duration);
    }

    //public async Task ZoomIn()
    //{
    //    while (camera_component.fieldOfView > defaultFoV)
    //    {
    //        camera_component.fieldOfView -= zoomStep;
    //        //await UniTask.NextFrame();
    //    }

    //    await Task.CompletedTask;
    //}

    //public async Task ZoomOut()
    //{
    //    float targetFov = 40f;
    //    while (camera_component.fieldOfView < targetFov)
    //    {
    //        camera_component.fieldOfView += zoomStep;
    //        //await UniTask.NextFrame();
    //    }
    //    await Task.CompletedTask;
    //}

    public void Shake(float duration, float magnitudeX = 0.1f, float magnitudeY = 0.1f)
    {
        shake_duration = duration;
        float x, y;
        if (shake_duration > 0)
        {
            x = Random.Range(-magnitudeX, magnitudeX);
            y = Random.Range(-magnitudeY, magnitudeY);
            gameObject.transform.position += Vector3.up * y + Vector3.right * x;
            shake_duration -= Time.deltaTime;
        }
    }

    public void Zoom(float target_zoom, float speed)
    {
        this.target_zoom = target_zoom;
        float FoV_diversity = camera_component.fieldOfView - target_zoom;
        if (FoV_diversity != 0) camera_component.fieldOfView *= (camera_component.fieldOfView / target_zoom) * speed * Time.deltaTime;
    }
}