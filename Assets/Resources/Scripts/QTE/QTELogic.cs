using System.Collections;
using UnityEngine;
using System;

namespace QTE
{
    public class QTELogic : MonoBehaviour
    {
        public static Action<QTEObject> OnStartQTE;

        [SerializeField] private Player _player;
        [SerializeField] private QTEBar _qTEBar;

        private float _qTERollback;
        private float _qTEForclick;
        private float _qTEStartingValue;

        private Coroutine _coroutineRollbackQTEBar;
        private float _currentValueQTEBar = 1f;

        private const float MinQTEBar = 0f;
        private const float MaxQTEBar = 1f;

        private void OnEnable() => OnStartQTE += StartQTE;

        private void StartQTE(QTEObject qTEObject)
        {
            _qTERollback = qTEObject.Rollback;
            _qTEForclick = qTEObject.Forclick;
            _qTEStartingValue = qTEObject.StartingValue;

            StartRollbackQTE();

            _player.Input.Player.QTE.performed += (context) => ForclickQTE();
        }

        private void ForclickQTE()
        {
            _currentValueQTEBar = Mathf.Clamp(_currentValueQTEBar + _qTEForclick, 0f, 1f);
        }

        private void StartRollbackQTE()
        {
            if (_coroutineRollbackQTEBar != null) 
                StopCoroutine(_coroutineRollbackQTEBar);

            _qTEBar.SetActiveQTEPanel(true);
            _coroutineRollbackQTEBar = StartCoroutine(RollbackQTEBar(_qTEBar, _qTERollback, _qTEStartingValue));
        }

        private IEnumerator RollbackQTEBar(QTEBar qte, float rollback, float startingValue)
        {
            _currentValueQTEBar = startingValue;
            while (_currentValueQTEBar > MinQTEBar && _currentValueQTEBar < MaxQTEBar)
            {
                qte.FillAmountQTEBar = _currentValueQTEBar;
                _currentValueQTEBar = Mathf.Clamp(_currentValueQTEBar - (rollback * Time.deltaTime), 0f, 1f);
                qte.FillAmountQTEBar = _currentValueQTEBar;
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.1f);

            QTEDetect.OnFinishActiveQTE?.Invoke(_currentValueQTEBar >= MaxQTEBar);

            qte.SetActiveQTEPanel(false);

            _player.Input.Player.QTE.performed -= (context) => ForclickQTE();
            _coroutineRollbackQTEBar = null;
        }

        private void OnDisable() => OnStartQTE -= StartQTE;
    }
}
