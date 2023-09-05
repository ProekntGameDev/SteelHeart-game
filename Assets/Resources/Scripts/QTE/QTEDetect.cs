using UnityEngine;
using Zenject;

namespace QTE
{
    public class QTEDetect : MonoBehaviour
    {
        [Inject] private Player _player;

        private bool _isUse = false;

        private QTEObject _qTEObject;

        private void OnTriggerStay(Collider other)
        {
            if (_isUse)
                return;

            if (_player.Input.Player.Interact.ReadValue<float>() <= 0)
                return;

            if (other.gameObject.TryGetComponent(out QTEObject qte))
            {
                if (qte.IsActive == false)
                    return;

                _qTEObject = qte;

                _isUse = true;
                QTELogic.OnStartQTE?.Invoke(_qTEObject);
                _qTEObject.OnEnd.AddListener(OnQteEnd);
            }
        }

        private void OnQteEnd(bool result)
        {
            _isUse = false;

            _qTEObject.OnEnd.RemoveListener(OnQteEnd);
            _qTEObject = null;
        }
    }
}
