using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace QTE
{
    public class QTEObject : MonoBehaviour
    {
        private const float MinQTE = 0f;
        private const float MaxQTE = 1f;

        public UnityEvent<float> OnProgressChanged;

        public bool IsActive { get; set; } = true;

        public float Rollback => _rollback;
        public float Forclick => _forclick;
        public float StartingValue => _startingValue;

        [SerializeField, Range(0f, 0.5f)] private float _rollback;
        [SerializeField, Range(0f, 1f)] private float _forclick;
        [SerializeField, Range(0f, 1f)] private float _startingValue;

        private Coroutine _coroutineRollbackQTE;
        private float _currentValue;

        public void StartQTE()
        {
            if (IsActive == false || _coroutineRollbackQTE != null)
                throw new System.InvalidOperationException();

            StartRollbackQTE();
        }

        public void ForclickQTE()
        {
            _currentValue = Mathf.Clamp(_currentValue + _forclick, MinQTE, MaxQTE);
        }

        private void StartRollbackQTE()
        {
            _coroutineRollbackQTE = StartCoroutine(RollbackQTEBar());
        }

        private IEnumerator RollbackQTEBar()
        {
            _currentValue = _startingValue;
            while (_currentValue > MinQTE && _currentValue < MaxQTE)
            {
                _currentValue = Mathf.Clamp(_currentValue - (_rollback * Time.deltaTime), MinQTE, MaxQTE);
                OnProgressChanged?.Invoke(_currentValue);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.1f);

            QTEDetect.OnFinishActiveQTE?.Invoke(_currentValue >= MaxQTE);
            _coroutineRollbackQTE = null;
        }
    }
}
