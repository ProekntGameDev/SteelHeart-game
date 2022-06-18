using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SteelHeart {
    public class CameraController : MonoBehaviour
    {

        public Transform target;
        public Vector3 offset;

        private void FixedUpdate()
        {
            transform.position = target.position - offset;
        }
    }
}