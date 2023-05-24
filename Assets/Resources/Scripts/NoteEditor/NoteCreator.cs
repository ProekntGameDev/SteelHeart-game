using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NoteEditor
{
    public class NoteCreator : MonoBehaviour
    {
        private Button _closeButton;
        private Button _addButton;
        private TMP_InputField _idField;
        private TMP_InputField _titleField;
        private TMP_InputField _messageField;
        
        private void Awake()
        {
            _closeButton = transform.Find("CloseButton").GetComponent<Button>();
            _closeButton.onClick.AddListener(Close);
            
            _addButton = transform.Find("AddButton").GetComponent<Button>();
            _addButton.onClick.AddListener(AddNote);
            
            _idField = transform.Find("IdField").GetComponent<TMP_InputField>();
            _titleField = transform.Find("TitleField").GetComponent<TMP_InputField>();
            _messageField = transform.Find("MessageScroll/Viewport/Content/MessageField").GetComponent<TMP_InputField>();
            
        }

        private void Start()
        {
            Hide();
        }

        private void AddNote()
        {
            int.TryParse(_idField.text, out int id);
            var note = new Note(id, _titleField.text, _messageField.text);
            var isSaved = NoteEditorTools.TrySave(note);

            _messageField.text = isSaved ? "SAVE SUCCESSFUL COMPLETE" : "NOTE DOES NOT SAVED";
        }
        
        private void Close()
        {
            Hide();
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}