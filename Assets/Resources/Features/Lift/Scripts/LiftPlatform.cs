using UnityEngine;

namespace Features.Lift
{
    public class LiftPlatform : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            other.transform.SetParent(transform);
        }

        private void OnTriggerExit(Collider other)
        {
            other.transform.SetParent(null);
        }
    }
}
