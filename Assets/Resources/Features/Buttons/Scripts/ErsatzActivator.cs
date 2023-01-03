using Common.Abstractions;
using UnityEngine;

namespace Features.Buttons.Ersatz
{
    public class ErsatzActivator : MonoBehaviour
    {
        [SerializeField, Range(.1f, 5)] private float _searchRadius = 1.0f;
        [SerializeField] private Color _gizmosColor = Color.blue;

        private void Update()
        {
            CheckInteractions();
        }

        private void CheckInteractions()
        {
            if (!Input.GetKeyDown(KeyCode.E))
                return;

            Collider[] crossedColliders = Physics.OverlapSphere(transform.position, _searchRadius);

            foreach (Collider collider in crossedColliders)
            {
                if (collider.transform.TryGetComponent(out IInteractable interactable))
                {
                    interactable.Interact();
                }
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = _gizmosColor;
            Gizmos.DrawWireSphere(Vector3.zero, _searchRadius);
        }
    }
}
