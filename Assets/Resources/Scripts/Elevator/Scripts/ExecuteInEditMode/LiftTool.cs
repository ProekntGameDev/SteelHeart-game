using UnityEngine;

namespace Features.Lift.Editor
{
    [ExecuteInEditMode]
    public class LiftTool : MonoBehaviour
    {
        [SerializeField] private LiftToolTip _upTip;
        [SerializeField] private LiftToolTip _downTip;
        [SerializeField] private LiftToolTip _platform;

        [SerializeField] private Color _gigmosColor = Color.yellow;
        [SerializeField] private Vector2 _gizmosRect = new Vector2(1f, 1f);

        private void Update()
        {
            if (Application.isPlaying)
                return;

            if (_upTip.transform.hasChanged)
                CheckTipUpPosition();

            if (_downTip.transform.hasChanged)
                CheckTipDownPosition();

            if (_platform.transform.hasChanged)
                CheckPlatformPosition();
        }

        private void CheckTipUpPosition()
        {
            _upTip.transform.position = new Vector3(transform.position.x,
                                                    _upTip.transform.position.y,
                                                    transform.position.z);

            if (_upTip.transform.position.y < transform.position.y)
                _upTip.transform.position = transform.position;

            if (_upTip.transform.position.y < _platform.transform.position.y)
                _platform.transform.position = _upTip.transform.position;
        }

        private void CheckTipDownPosition()
        {
            _downTip.transform.position = transform.position;
        }

        private void CheckPlatformPosition()
        {
            _platform.transform.position = new Vector3(transform.position.x,
                                                    _platform.transform.position.y,
                                                    transform.position.z);

            if (_platform.transform.position.y < transform.position.y)
                _platform.transform.position = transform.position;

            if (_platform.transform.position.y > _upTip.transform.position.y)
                _platform.transform.position = _upTip.transform.position;
        }

        void OnDrawGizmos()
        {
            return;

            // todo: исправить неправильную отрисовку Gizmos

            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.color = _gigmosColor;

            Vector3 tipsCenter = (_upTip.transform.position + _downTip.transform.position) / 2;
            float midleY = tipsCenter.y;

            float gizmosHeight = (_upTip.transform.position - _downTip.transform.position).magnitude;

            Gizmos.DrawWireCube(midleY * Vector3.up,
                                new Vector3(_gizmosRect.x, gizmosHeight, _gizmosRect.y));
        }
    }
}
