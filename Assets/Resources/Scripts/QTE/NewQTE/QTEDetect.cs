using System;
using UnityEngine;
using NaughtyAttributes;

namespace QTE
{
    public class QTEDetect : MonoBehaviour
    {
        public static Action<bool> OnFinishActiveQTE;

        public bool IsWin { get; private set; } = false;

        [SerializeField, Required] private Player _player;

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
                if (_player.Input.Player.Interact.ReadValue<float>() > 0 && other.gameObject.TryGetComponent(out QTEObject qte))
                {
                    _qTEObject = qte;

                    if (_qTEObject.IsActive)
                    {
                        _isUse = true;
                        QTELogic.OnStartQTE?.Invoke(_qTEObject);
                    }
                }
            }
        }

        private void OnDisable() => OnFinishActiveQTE -= SetIsWin;
    }
}
