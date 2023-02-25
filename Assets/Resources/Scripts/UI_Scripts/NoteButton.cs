using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SteelHeart
{
    [RequireComponent(typeof(Button))]
    public class NoteButton : MonoBehaviour
    {
        public TextMeshProUGUI noteText;
        public GameMeta.Note noteData;

        private Button _button;
        private TextMeshProUGUI _buttonText;


        private void Awake()
        {
            _buttonText = GetComponentInChildren<TextMeshProUGUI>();
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _button.onClick.AddListener(OpenNote);
        }

        public void SetText()
        {
            // _buttonText.text = noteData.title;
        }

        public void OpenNote()
        {
            // noteText.text = noteData.text;
            noteText.transform.parent.gameObject.SetActive(true);
            transform.parent.gameObject.SetActive(false);
        }
    }
}
