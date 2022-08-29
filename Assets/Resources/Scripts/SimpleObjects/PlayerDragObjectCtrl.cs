using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SteelHeart
{
    public class PlayerDragObjectCtrl : MonoBehaviour
    {
        public Transform finish_transform;
        public float speed;
        public float cooldown;

        private Vector3 start_pos;
        private Vector3 direction;
        private Ray ray;
        private RaycastHit[] hits;
        private RaycastHit hit;
        private float distance;
        private float least_distance;
        private Collider _collider;
        private float counter;
        private bool isUsing;

        void Start()
        {
            _collider = gameObject.GetComponent<Collider>();
            _collider.enabled = false;

            start_pos = gameObject.transform.position;
            distance = Vector3.Distance(start_pos, finish_transform.position);
            direction = finish_transform.position - start_pos;
            ray = new Ray(start_pos, direction);
        }

        void Update()
        {
            if (counter > 0)
            {
                counter -= Time.deltaTime;
                return;
            }

            if (isUsing == false) _collider.enabled = false;

            bool isHitsPlayer = false;
            hits = Physics.RaycastAll(ray, distance);
            if (hits == null) return;
            for (int i = 0; i < hits.Length; ++i)
            {
                isHitsPlayer = hits[i].collider.gameObject.tag == "player";
                if (isHitsPlayer)
                {
                    hit = hits[i];
                    break;
                }
            }

            if (isHitsPlayer || isUsing)
            {
                _collider.enabled = true;
                if (isUsing == false)
                {
                    isUsing = true;
                    gameObject.transform.position = hit.point;
                    least_distance = Vector3.Distance(gameObject.transform.position, finish_transform.position);
                }
                else
                {
                    least_distance -= speed * Time.deltaTime;
                    gameObject.transform.position = start_pos + direction * (1 - (least_distance / distance));
                    if (least_distance / distance <= 0)
                    {
                        isUsing = false;
                        gameObject.transform.position = start_pos;
                        counter = cooldown;
                    }
                }

            }
        }
    }
}
