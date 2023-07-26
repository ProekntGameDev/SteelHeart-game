using UnityEngine;

namespace QTE
{
    public class QTEObject : MonoBehaviour
    {
        [Range(0f, 0.05f)] public float Rollback;
        [Range(0f, 1f)] public float Forclick;
        [Range(0f, 1f)] public float StartingValue;

        public bool IsActive { get; private set; } = true;

        public void SetIsActive(bool isActive) => IsActive = isActive;
    }
}
