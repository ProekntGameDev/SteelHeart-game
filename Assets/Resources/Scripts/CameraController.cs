using UnityEngine;
using Random = UnityEngine.Random;

public class CameraController : MonoBehaviour
{
    public Vector3 offset;
    private Camera camera_component;
    private float defaultFoV;
    //
    private Transform target_transform;
    //
    private float shake_duration;
    private float last_x_magnitude = 0;
    private float last_y_magnitude = 0;
    private float target_FoV;
    public float zoom_speed;

    private void Start()
    {
        Application.targetFrameRate = 144;
        target_transform = GameObject.Find("Player").transform;
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

    public void Shake(float duration, float magnitudeX = 0, float magnitudeY = 0)
    {
        if (magnitudeX == 0 && magnitudeY == 0)
        {
            magnitudeX = last_x_magnitude;
            magnitudeY = last_y_magnitude;
        }
        last_x_magnitude = magnitudeX;
        last_y_magnitude = magnitudeY;

        shake_duration = duration;
        float x, y;
        if (shake_duration > 0)
        {
            x = Random.Range(-magnitudeX, magnitudeX);
            y = Random.Range(-magnitudeY, magnitudeY);
            transform.position += Vector3.up * y + Vector3.right * x;
            shake_duration -= Time.deltaTime;
        }
    }

    public void Zoom(float target_FoV)
    {
        this.target_FoV = target_FoV;
        float FoV_diversity = target_FoV - camera_component.fieldOfView;
        if (FoV_diversity != 0) camera_component.fieldOfView += FoV_diversity * zoom_speed * Time.deltaTime;
    }

    public void NightVisionEffectActiveStateChange() 
    {
        ObjectActiveChange("GreenFilter");
        ObjectActiveChange("Spotlight");

        void ObjectActiveChange(string name)
        {
            GameObject[] objects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
            for (int i = 0; i < objects.Length; ++i) 
            if (objects[i].name == name) 
            { objects[i].SetActive(!objects[i].activeSelf); break; }
        }
    }
}