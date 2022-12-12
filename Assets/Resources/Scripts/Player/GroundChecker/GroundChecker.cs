using UnityEngine;

namespace GroundCheck
{
    public class GroundChecker : MonoBehaviour
    {
        [SerializeField] private float _distance = 0.2f;
        [SerializeField] private LayerMask _layerMask = ~0;

#if UNITY_EDITOR
        [Header("Debug.DrawRay")]
        [SerializeField] private Color _hitColor = Color.green;
        [SerializeField] private Color _missColor = Color.red;
#endif

        private SurfaceSensor[] _sensors;

        public bool IsGrounded
        {
            get
            {
                foreach (var sensor in _sensors)
                {
                    if (sensor.IsNearSurface)
                        return true;
                }
                return false;
            }
        }

        private void Start()
        {
            _sensors = GetComponentsInChildren<SurfaceSensor>();

            foreach (var sensor in _sensors)
                sensor.Init(_distance, _layerMask);

#if UNITY_EDITOR
            SurfaceSensorDebugDrawer[] drawers = GetComponentsInChildren<SurfaceSensorDebugDrawer>();
            foreach (var drawer in drawers)
                drawer.Init(_distance, _hitColor, _missColor);
#endif
        }
    }
}
