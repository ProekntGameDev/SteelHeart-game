using UnityEngine;
using UnityEngine.UI;

namespace QTE
{
    public class QTEBar : MonoBehaviour
    {
        [SerializeField] private GameObject _qTEPanel;
        [SerializeField] private Image _qTEBar;

        public void SetFillAmountQTEBar(float fillAmount) => _qTEBar.fillAmount = fillAmount;

        public float GetFillAmountQTEBar() { return _qTEBar.fillAmount; }

        public void SetActiveQTEPanel(bool isActive) => _qTEPanel.SetActive(isActive);
    }
}
