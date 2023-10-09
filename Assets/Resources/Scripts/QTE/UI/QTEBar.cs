using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
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
        [SerializeField] private QTEButtons _qTEButtonsData;
        [SerializeField] private TextMeshProUGUI _qTEButton;

        public void EnableQTEPanel(InputBinding inputBinding)
        {
            _qTEPanel.SetActive(true);
            _qTEButton.gameObject.SetActive(true);

            _qTEButtonsData.SetIconOrText(_qTEButton, inputBinding);
        }

        public void DisableQTEPanel()
        {
            _qTEPanel.SetActive(false);
            _qTEButton.gameObject.SetActive(false);

            _qTEButton.text = "";
        }
    }
}
