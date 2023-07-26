using UnityEngine;
using UnityEngine.UI;

namespace QTE
{
    public class QTEBar : MonoBehaviour
    {
        public float FillAmountQTEBar
        {
            get { return _qTEBar.fillAmount; }
            set { _qTEBar.fillAmount = Mathf.Clamp(value, 0f, 1f); }
        }

        [SerializeField] private GameObject _qTEPanel;
        [SerializeField] private Image _qTEBar;

        public void SetActiveQTEPanel(bool isActive) => _qTEPanel.SetActive(isActive);
    }
}
