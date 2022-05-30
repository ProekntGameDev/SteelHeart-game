using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingTrap : MonoBehaviour
{
    public float time;
    public float counter;
    float direction = 1;
    Vector3 start_scale;
    Vector3 start_pos;
    private void Start()
    {
        start_scale = gameObject.transform.localScale;
        start_pos = gameObject.transform.position;
    }
    void Update()
    {
        float max_scale_offset = 1 * start_scale.y;
        float max_pos_offset = 0.5f * start_scale.y;
        if (counter < 0)
        {
            counter = time * -direction;
            direction *= -1;
        }
        else
        {
            counter -= Time.deltaTime*direction;
            gameObject.transform.localScale = start_scale + Vector3.up * ((direction * start_scale.y * (counter / time)) - 1* start_scale.y);
            gameObject.transform.position = start_pos + Vector3.up * ((0.5f * direction * start_scale.y * (counter / time)) - 0.5f);
        }
    }
}
