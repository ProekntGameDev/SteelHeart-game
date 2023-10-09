using UnityEngine;
using System;
using Zenject;
using Random = UnityEngine.Random;
using UnityEngine.InputSystem;

namespace QTE
{
    public class QTELogic : MonoBehaviour
    {
        public static Action<QTEObject> OnStartQTE;

        [SerializeField] private QTEBar _qTEBar;

        [Inject] private Player _player;

        private InputBinding _inputBinding;
        private QTEObject _qTEObject;

        private void StartQTE(QTEObject qTEObject)
        {
            _inputBinding = _player.Input.Player.QTE.bindings[Random.Range(0, _player.Input.Player.QTE.bindings.Count)];
            _qTEObject = qTEObject;

            qTEObject.StartQTE(_inputBinding);

            _qTEBar.EnableQTEPanel(_inputBinding);

            _player.Input.Player.QTE.performed += QTEPerformed;
            qTEObject.OnEnd.AddListener(EndQTE);
            qTEObject.OnProgressChanged.AddListener((newValue) => _qTEBar.FillAmountQTEBar = newValue);
        }

        private void EndQTE(bool result)
        {
            _player.Input.Player.QTE.performed -= QTEPerformed;
            _qTEObject.OnEnd.RemoveListener(EndQTE);
            _qTEObject.OnProgressChanged.RemoveListener((newValue) => _qTEBar.FillAmountQTEBar = newValue);

            _qTEBar.DisableQTEPanel();

            _qTEObject = null;
            _inputBinding = default;
        }

        private void QTEPerformed(InputAction.CallbackContext context)
        {
            if (_qTEObject == null || context.action.GetBindingForControl(context.control) != _inputBinding)
                return;

            _qTEObject.ForclickQTE();
        }

        private void OnEnable() => OnStartQTE += StartQTE;
        private void OnDisable() => OnStartQTE -= StartQTE;
    }
}
