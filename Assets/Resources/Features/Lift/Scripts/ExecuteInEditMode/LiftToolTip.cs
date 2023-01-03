using UnityEngine;

namespace Features.Lift.Editor
{
    [ExecuteInEditMode]
    public class LiftToolTip : MonoBehaviour
    {
        [SerializeField] private bool _wireframe = false;
        [SerializeField] private Color _gizmosColor = Color.blue;
        [SerializeField] private Vector3 _gizmosRect = new Vector3(.5f, .5f, .5f);

        void OnDrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.color = _gizmosColor;

            if (_wireframe)
                Gizmos.DrawWireCube(Vector3.zero, _gizmosRect);
            else
                Gizmos.DrawCube(Vector3.zero, _gizmosRect);
        }
    }
}
