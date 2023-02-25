using SteelHeart;
using UnityEngine;
using UnityEngine.UI;

namespace NoteEditor
{
    public class NoteEditor : MonoBehaviour
    {
        private Button _redactorButton;
        private NoteRedactor _redactorPanel;

        private Button _creatorButton;
        private NoteCreator _creatorPanel;

        private void Awake()
        {
            GameData.Note.Initialize();
            _redactorButton = transform.Find("RedactorButton").GetComponent<Button>();
            _redactorButton.onClick.AddListener(OpenRedactor);
            _redactorPanel = GetComponentInChildren<NoteRedactor>();
            
            _creatorButton = transform.Find("CreatorButton").GetComponent<Button>();
            _creatorButton.onClick.AddListener(OpenCreator);
            _creatorPanel = GetComponentInChildren<NoteCreator>();
        }

        private void OpenRedactor()
        {
            _redactorPanel.Show();
        }

        private void OpenCreator()
        {
            _creatorPanel.Show();
        }
    }
}