using System.Collections;
using UnityEngine;
using System;

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
            _currentValueQTEBar = CheckFillAmount(_currentValueQTEBar + _qTEForclick);
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
                qte.SetFillAmountQTEBar(_currentValueQTEBar);
                _currentValueQTEBar = CheckFillAmount(_currentValueQTEBar - rollback);
                yield return new WaitForSeconds(0.01f);
            }
            qte.SetFillAmountQTEBar(_currentValueQTEBar);
            yield return new WaitForSeconds(0.1f);

            if (_currentValueQTEBar >= MaxQTEBar) QTEDetect.OnFinishActiveQTE?.Invoke(true);
            else QTEDetect.OnFinishActiveQTE?.Invoke(false);

            qte.SetActiveQTEPanel(false);
            if (_coroutineRollbackQTEBar != null) StopCoroutine(_coroutineRollbackQTEBar);
        }

        private float CheckFillAmount(float value)
        {
            if (value > 1) return 1f;
            else if (value < 0) return 0f;
            return value;
        }

        private void OnDisable() => OnStartQTE -= StartQTE;
    }
}
