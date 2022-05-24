using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraController : MonoBehaviour
{
    public Transform target_transform;
    public Vector3 offset;
    private Camera camera_component;
    private float defaultFoV;
    //
    private float shake_duration;
    private float target_FoV;
    public float zoom_speed;

    private void Start()
    {
        Application.targetFrameRate = 144;
    }
    private void Awake()
    {
        camera_component = gameObject.GetComponent<Camera>();
        target_FoV = defaultFoV = camera_component.fieldOfView;
    }

    private void LateUpdate()
    {
        if (target_transform) base.transform.position = target_transform.position - offset;
        if (shake_duration > 0) Shake(shake_duration);
        if (target_FoV != camera_component.fieldOfView) Zoom(target_FoV);
    }

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

    public void Zoom(float target_FoV)
    {
        this.target_FoV = target_FoV;
        float FoV_diversity = target_FoV - camera_component.fieldOfView;
        if (FoV_diversity != 0) camera_component.fieldOfView += FoV_diversity * zoom_speed * Time.deltaTime;
    }
}