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

        private void OnEnable() => OnStartQTE += StartQTE;

        private void StartQTE(QTEObject qTEObject)
        {
            qTEObject.StartQTE();

            _qTEBar.SetActiveQTEPanel(true);

            _player.Input.Player.QTE.performed += (context) => qTEObject.ForclickQTE();
            QTEDetect.OnFinishActiveQTE += (result) => EndQTE(qTEObject);
            qTEObject.OnProgressChanged.AddListener((newValue) => _qTEBar.FillAmountQTEBar = newValue);
        }

        private void EndQTE(QTEObject qTEObject)
        {
            _player.Input.Player.QTE.performed -= (context) => qTEObject.ForclickQTE();
            QTEDetect.OnFinishActiveQTE -= (result) => EndQTE(qTEObject);
            qTEObject.OnProgressChanged.RemoveListener((newValue) => _qTEBar.FillAmountQTEBar = newValue);

            _qTEBar.SetActiveQTEPanel(false);
        }

        private void OnDisable() => OnStartQTE -= StartQTE;
    }
}
