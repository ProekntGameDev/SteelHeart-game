using UnityEngine;
using UnityEngine.Events;

namespace Features.Lift
{
    public class LiftPlatform : MonoBehaviour
    {
        public UnityEvent OnEnter;
        public UnityEvent OnExit;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player) == false)
                return;

            other.transform.SetParent(transform);
            OnEnter?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Player player) == false)
                return;

            other.transform.SetParent(null);
            OnExit?.Invoke();
        }
    }
}
