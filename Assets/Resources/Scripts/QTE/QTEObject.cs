using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace QTE
{
    public class QTEObject : Interactable
    {
        private const float MinQTE = 0f;
        private const float MaxQTE = 1f;

        [HideInInspector] public UnityEvent<float> OnProgressChanged;
        public UnityEvent OnStart;
        public UnityEvent<bool> OnEnd;

        public bool IsActive { get; set; } = true;

        public float Rollback => _rollback;
        public float Forclick => _forclick;
        public float StartingValue => _startingValue;

        [SerializeField, Range(0f, 0.5f)] private float _rollback;
        [SerializeField, Range(0f, 1f)] private float _forclick;
        [SerializeField, Range(0f, 1f)] private float _startingValue;

        private InputBinding _inputBinding;
        private Coroutine _coroutineRollbackQTE;
        private float _currentValue;

        public void StartQTE(InputBinding inputBinding)
        {
            if (IsActive == false || _coroutineRollbackQTE != null)
                throw new System.InvalidOperationException();

            _inputBinding = inputBinding;
            StartRollbackQTE();
            OnStart?.Invoke();
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
            CanInteract = false;

            _currentValue = _startingValue;
            while (_currentValue > MinQTE && _currentValue < MaxQTE)
            {
                _currentValue = Mathf.Clamp(_currentValue - (_rollback * Time.deltaTime), MinQTE, MaxQTE);
                OnProgressChanged?.Invoke(_currentValue);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.1f);

            bool qteResult = _currentValue >= MaxQTE;

            IsActive = !qteResult;
            _coroutineRollbackQTE = null;

            if (qteResult == false)
                CanInteract = true;

            OnEnd?.Invoke(qteResult);
        }
    }
}
