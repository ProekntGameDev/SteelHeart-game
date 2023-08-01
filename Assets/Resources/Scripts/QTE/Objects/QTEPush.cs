using NaughtyAttributes;
using UnityEngine;

namespace QTE
{
    public class QTEPush : MonoBehaviour
    {
        [SerializeField, Required] private Transform _target;
        [SerializeField, Required] private QTEObject _qteTarget;
        [SerializeField, Required] private Transform _startPoint;
        [SerializeField, Required] private Transform _endPoint;

        private void Start()
        {
            _qteTarget.OnProgressChanged.AddListener(QTEUpdate);
        }

        private void QTEUpdate(float newValue)
        {
            _target.transform.position = Vector3.Lerp(_startPoint.position, _endPoint.position, newValue);
        }
    }
}
