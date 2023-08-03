using UnityEngine;

namespace QTE
{
    [RequireComponent(typeof(QTEObject))]
    public abstract class QTEBehavior : MonoBehaviour
    {
        protected QTEObject QTEObject { get; private set; }

        public abstract void OnStart();

        public abstract void OnProgressChanged(float progress);

        public abstract void OnEnd(bool result);

        private void Awake()
        {
            QTEObject = GetComponent<QTEObject>();

            QTEObject.OnStart.AddListener(OnStart);
            QTEObject.OnProgressChanged.AddListener(OnProgressChanged);
            QTEObject.OnEnd.AddListener(OnEnd);
        }
    }
}
