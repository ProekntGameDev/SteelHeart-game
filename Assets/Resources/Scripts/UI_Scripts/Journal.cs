using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SteelHeart
{
    public class Journal : MonoBehaviour
    {
        public GameObject journalGameObject;
        public GameObject notesListPanel;
        public GameObject noteTextPanel;
        public Button closeNoteButton;
        public NoteButton noteButtonPrefab;
        [Space]
        public KeyCode openJournalKey = KeyCode.J;

        private TextMeshProUGUI _noteText;

        private void Awake()
        {
            _noteText = noteTextPanel.GetComponentInChildren<TextMeshProUGUI>();
            closeNoteButton.onClick.AddListener(() =>
            {
                notesListPanel.SetActive(true);
                noteTextPanel.SetActive(false);
            });
        }

        private void Update()
        {
            if (Input.GetKeyDown(openJournalKey))
            {
                OpenOrClose();
            }
        }

        public void OpenOrClose()
        {
            if (journalGameObject.activeSelf == false)
            {
                UpdateNotes();
                notesListPanel.SetActive(true);
                noteTextPanel.SetActive(false);
            }
            journalGameObject.SetActive(!journalGameObject.activeSelf);
        }

        private void UpdateNotes()
        {
            foreach (Transform noteButton in notesListPanel.transform)
            {
                Destroy(noteButton.gameObject);
            }

            foreach (var noteData in GameData.Note.CollectedNotes)
            {
                NoteButton button = Instantiate(noteButtonPrefab);
                button.transform.parent = notesListPanel.transform;
                button.noteData = noteData;
                button.noteText = _noteText;
                button.SetText();
            }
        }
    }

}