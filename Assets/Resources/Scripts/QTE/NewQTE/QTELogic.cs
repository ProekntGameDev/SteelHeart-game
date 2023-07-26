using System.Collections;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace QTE
{
    public class QTELogic : MonoBehaviour
    {
        public static Action<float, float, float> OnStartQTE;

        [SerializeField] private KeyCode _buttonClick = KeyCode.Tab;
        [SerializeField] private QTEBar _qTEBar;

        private float _qTERollback;
        private float _qTEForclick;
        private float _qTEStartingValue;

        private Coroutine _coroutineRollbackQTEBar;
        private float _currentValueQTEBar = 1f;

        private const float MinQTEBar = 0f;
        private const float MaxQTEBar = 1f;

        private void OnEnable() => OnStartQTE += StartQTE;

        private void StartQTE(float rollback, float forclick, float startValue)
        {
            _qTERollback = rollback;
            _qTEForclick = forclick;
            _qTEStartingValue = startValue;

            StartRollbackQTE();
        }

        private void Update()
        {
            if (Input.GetKeyDown(_buttonClick)) ForclickQTE();
        }

        private void ForclickQTE()
        {
            _currentValueQTEBar = Mathf.Clamp(_currentValueQTEBar + _qTEForclick, 0f, 1f);
        }

        private void StartRollbackQTE()
        {
            if (_coroutineRollbackQTEBar != null) StopCoroutine(_coroutineRollbackQTEBar);

            _qTEBar.SetActiveQTEPanel(true);
            _coroutineRollbackQTEBar = StartCoroutine(RollbackQTEBar(_qTEBar, _qTERollback, _qTEStartingValue));
        }

        private IEnumerator RollbackQTEBar(QTEBar qte, float rollback, float startingValue)
        {
            _currentValueQTEBar = startingValue;
            while (_currentValueQTEBar > MinQTEBar && _currentValueQTEBar < MaxQTEBar)
            {
                qte.FillAmountQTEBar = _currentValueQTEBar;
                _currentValueQTEBar = Mathf.Clamp(_currentValueQTEBar - rollback, 0f, 1f);
                yield return new WaitForSeconds(0.01f);
            }
            qte.FillAmountQTEBar = _currentValueQTEBar;
            yield return new WaitForSeconds(0.1f);

            QTEDetect.OnFinishActiveQTE?.Invoke(_currentValueQTEBar >= MaxQTEBar);

            qte.SetActiveQTEPanel(false);
            if (_coroutineRollbackQTEBar != null) StopCoroutine(_coroutineRollbackQTEBar);
        }

        private void OnDisable() => OnStartQTE -= StartQTE;
    }
}
