using UnityEngine;

namespace QTE
{
    public class QTEObject : MonoBehaviour
    {
        public float Rollback => _rollback;
        public float Forclick => _forclick;
        public float StartingValue => _startingValue;

        [SerializeField, Range(0f, 0.5f)] private float _rollback;
        [SerializeField, Range(0f, 1f)] private float _forclick;
        [SerializeField, Range(0f, 1f)] private float _startingValue;

        public bool IsActive { get; private set; } = true;

        public void SetIsActive(bool isActive) => IsActive = isActive;
    }
}
