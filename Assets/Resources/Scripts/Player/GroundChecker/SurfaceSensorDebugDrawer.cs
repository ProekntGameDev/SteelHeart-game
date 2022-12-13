using UnityEngine;

namespace GroundCheck
{
#if UNITY_EDITOR
    /// <summary>
    /// Компонент, отображающий направление и результат работы регистратора поверхности
    /// (работает только в редакторе)
    /// </summary>
    [RequireComponent(typeof(SurfaceSensor))]
    public class SurfaceSensorDebugDrawer : MonoBehaviour
    {
        private SurfaceSensor _sensor;

        private float _distance;
        private Color _hitColor = Color.green;
        private Color _missColor = Color.red;

        public void Init(float distance, Color hitColor, Color missColor)
        {
            _distance = distance;
            _hitColor = hitColor;
            _missColor = missColor;
        }

        private SurfaceSensor Sensor
        {
            get
            {
                if (_sensor == null)
                    _sensor = GetComponent<SurfaceSensor>();

                return _sensor;
            }
        }

        private void OnDrawGizmos()
        {
            if (Sensor.IsNearSurface)
                Debug.DrawRay(transform.position, (-1) * _distance * transform.up, _hitColor);
            else
                Debug.DrawRay(transform.position, (-1) * _distance * transform.up, _missColor);
        }
    }
#endif
}
