using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class JournalUI : BaseMenu
{
    protected override InputAction _menuButton => _player.Input.PlayerUI.Journal;

    [SerializeField, Required] private Transform _scrollViewContent;
    [SerializeField, Required] private JournalNoteUI _textNotePrefab;
    [SerializeField, Required] private JournalNoteUI _audioNotePrefab;
    [SerializeField, Required] private JournalContent _content;

    private void Start()
    {
        _player.Journal.OnNoteAdded.AddListener(AddNoteUI);
    }

    private void AddNoteUI(NoteData noteData)
    {
        JournalNoteUI notePrefab = noteData.NoteContent.Audio == null ? _textNotePrefab : _audioNotePrefab;

        JournalNoteUI journalNoteUI = Instantiate(notePrefab, _scrollViewContent.transform);
        journalNoteUI.Init(_content, noteData);
    }

    protected override void EscapePerformed(InputAction.CallbackContext context)
    {
        if (_panel.activeInHierarchy == false)
            return;

        Disable();
    }
}
