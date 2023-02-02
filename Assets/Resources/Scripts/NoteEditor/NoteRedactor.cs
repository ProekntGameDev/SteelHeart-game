using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NoteEditor
{
    public class NoteRedactor : MonoBehaviour
    {
        private Button _closeButton;
        private Button _saveButton;
        private Button _searchButton;
        private TMP_InputField _idField;
        private TMP_InputField _titleField;
        private TMP_InputField _messageField;

        private SteelHeart.GameMeta.NoteData _noteData;

        private void Awake()
        {
            _closeButton = transform.Find("CloseButton").GetComponent<Button>();
            _closeButton.onClick.AddListener(Close);

            _saveButton = transform.Find("SaveButton").GetComponent<Button>();
            _saveButton.onClick.AddListener(Save);

            _searchButton = transform.Find("SearchButton").GetComponent<Button>();
            _searchButton.onClick.AddListener(Search);

            _idField = transform.Find("IdField").GetComponent<TMP_InputField>();
            _titleField = transform.Find("TitleField").GetComponent<TMP_InputField>();
            _messageField = transform.Find("MessageScroll/Viewport/Content/MessageField")
                .GetComponent<TMP_InputField>();
        }

        private void Start()
        {
            Hide();
        }

        private void Search()
        {
            _noteData = NoteEditorTools.IsIdValid(_idField.text)
                ? SteelHeart.GameData.Note.GetNoteById(Convert.ToInt32(_idField.text))
                : NoteEditorTools.IsTitleValid(_titleField.text)
                    ? SteelHeart.GameData.Note.GetNoteByTitle(_titleField.text)
                    : null;

            if (_noteData != null)
            {
                _idField.text = _noteData.Id.ToString();
                _titleField.text = _noteData.Title;
                _messageField.text = _noteData.Message;
            }
        }

        private void Save()
        {
            if (NoteEditorTools.IsDataValid(_idField.text, _titleField.text, _messageField.text))
            {
                var note = new Note(Convert.ToInt32(_idField.text), _titleField.text, _messageField.text);
                NoteEditorTools.Redact(_noteData, note);
                _messageField.text = "SAVE SUCCESSFUL COMPLETE";
            }
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