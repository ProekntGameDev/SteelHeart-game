using UnityEngine;

namespace GroundCheck
{
    /// <summary>
    /// Компонент, регистрирующий поверхность в пределах указанного расстояния
    /// </summary>
    public class SurfaceSensor : MonoBehaviour
    {
        private float _distance = 1.0f;
        private LayerMask _layerMask = ~0;

        public void Init(float distance, LayerMask layerMask)
        {
            _distance = distance;
            _layerMask = layerMask;
        }

        public bool IsNearSurface =>
            Physics.Raycast(
                transform.position,
                (-1) * transform.up,
                _distance,
                _layerMask);
    }
}
