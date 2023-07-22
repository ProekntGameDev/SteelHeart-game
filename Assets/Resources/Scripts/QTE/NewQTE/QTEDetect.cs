using System;
using UnityEngine;
using System.Collections;

namespace QTE
{
    public class QTEDetect : MonoBehaviour
    {
        public static Action<bool> OnFinishActiveQTE;

        public bool IsWin { get; private set; } = false;

        [SerializeField] private string _qTETag = "QTE";
        [SerializeField] private KeyCode _button = KeyCode.E;

        private bool _isUse = false;

        private QTEObject _qTEObject;

        private void OnEnable() => OnFinishActiveQTE += SetIsWin;

        private void SetIsWin(bool isUse)
        {
            _isUse = false;
            IsWin = isUse;

            _qTEObject.SetIsActive(!IsWin);
        }

        private void OnTriggerStay(Collider other)
        {
            if (!_isUse)
            {
                if (Input.GetKeyDown(_button) && other.gameObject.tag == _qTETag)
                {
                    if (other.gameObject.TryGetComponent(out QTEObject qte))
                    {
                        _qTEObject = qte;

                        if (_qTEObject.IsActive)
                        {
                            _isUse = true;
                            QTELogic.OnStartQTE?.Invoke(_qTEObject.Rollback, _qTEObject.Forclick, _qTEObject.StartingValue);
                        }
                    }
                }
            }
        }

        private void OnDisable() => OnFinishActiveQTE -= SetIsWin;
    }
}
