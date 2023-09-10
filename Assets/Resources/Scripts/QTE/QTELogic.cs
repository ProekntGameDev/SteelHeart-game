using UnityEngine;
using System;
using Zenject;

namespace QTE
{
    public class QTELogic : MonoBehaviour
    {
        public static Action<QTEObject> OnStartQTE;

        [SerializeField] private QTEBar _qTEBar;

        [Inject] private Player _player;

        private void StartQTE(QTEObject qTEObject)
        {
            qTEObject.StartQTE();

            _qTEBar.SetActiveQTEPanel(true);

            _player.Input.Player.QTE.performed += (context) => qTEObject.ForclickQTE();
            qTEObject.OnEnd.AddListener((result) => EndQTE(qTEObject));
            qTEObject.OnProgressChanged.AddListener((newValue) => _qTEBar.FillAmountQTEBar = newValue);
        }

        private void EndQTE(QTEObject qTEObject)
        {
            _player.Input.Player.QTE.performed -= (context) => qTEObject.ForclickQTE();
            qTEObject.OnEnd.RemoveListener((result) => EndQTE(qTEObject));
            qTEObject.OnProgressChanged.RemoveListener((newValue) => _qTEBar.FillAmountQTEBar = newValue);

            _qTEBar.SetActiveQTEPanel(false);
        }

        private void OnEnable() => OnStartQTE += StartQTE;
        private void OnDisable() => OnStartQTE -= StartQTE;
    }
}
