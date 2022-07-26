using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingTrap : MonoBehaviour
{
    public float uplift_time;
    public float downlift_time;
    float c_time;
    public float waiting;
    float counter;
    float wcounter;
    Vector3 start_scale;
    Vector3 start_pos;
    MeshFilter mfilter;
    MeshRenderer mrender;

    int direction = 1;
    private void Start()
    {
        Ray ray = new Ray(gameObject.transform.position, -gameObject.transform.up);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        gameObject.transform.position = hit.point + 0.5f * gameObject.transform.up;

        start_scale = gameObject.transform.localScale;
        start_pos = gameObject.transform.position;

        wcounter = waiting;
        counter = c_time = (direction == 1) ? uplift_time : downlift_time;
    }
    void Update()
    {
        if (counter < 0)
        {
            if (wcounter > 0) { wcounter -= Time.deltaTime; }
            else
            {
                wcounter = waiting;
                c_time = (direction == 1) ? uplift_time : downlift_time;
                counter = c_time;
                direction *= -1;
            }
        }
        else
        {
            counter -= Time.deltaTime;
            float least_time = (direction == 1) ? counter / c_time : 1 - counter / c_time;

            //gameObject.transform.localScale = start_scale + Vector3.up * ((start_scale.y * least_time) - 1 * start_scale.y);
            //gameObject.transform.position = start_pos + Vector3.up * ((0.5f * start_scale.y * least_time) - 0.5f);

            float height = start_scale.y * least_time - 1f;
            gameObject.transform.position = start_pos + gameObject.transform.up * ((height > 0) ? 0:height);
        }
    }
}
