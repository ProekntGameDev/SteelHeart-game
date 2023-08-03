using NaughtyAttributes;
using UnityEngine;

public class JournalUI : MonoBehaviour
{
    [SerializeField, Required] private Player _player;
    [SerializeField, Required] private Journal _journal;
    [SerializeField, Required] private GameObject _panel;
    [SerializeField, Required] private Transform _scrollViewContent;
    [SerializeField, Required] private JournalNoteUI _textNotePrefab;
    [SerializeField, Required] private JournalNoteUI _audioNotePrefab;
    [SerializeField, Required] private JournalContent _content;

    private void Start()
    {
        _journal.OnNoteAdded.AddListener(AddNoteUI);
    }

    private void AddNoteUI(NoteData noteData)
    {
        JournalNoteUI notePrefab = noteData.NoteContent.Audio == null ? _textNotePrefab : _audioNotePrefab;

        JournalNoteUI journalNoteUI = Instantiate(notePrefab, _scrollViewContent.transform);
        journalNoteUI.Init(_content, noteData);
    }

    private void OnEnable()
    {
        _player.Input.Player.Journal.performed += (c) => _panel.SetActive(!_panel.activeInHierarchy);
    }

    private void OnDisable()
    {
        _player.Input.Player.Journal.performed -= (c) => _panel.SetActive(!_panel.activeInHierarchy);
    }
}
