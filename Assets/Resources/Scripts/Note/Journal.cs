using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Zenject;

public class Journal : MonoBehaviour
{
    public UnityEvent<NoteData> OnNoteAdded;

    public IReadOnlyList<NoteData> Notes => _notes;

    [Inject] private Player _player;

    private List<NoteData> _notes = new List<NoteData>();

    public void AddNote(NoteData noteData)
    {
        if (noteData == null)
            throw new System.ArgumentNullException(nameof(noteData));

        _notes.Add(noteData);
        OnNoteAdded?.Invoke(noteData);
    }

    private void OverlapTrigger(InputAction.CallbackContext context)
    {
        if (_player.Interactor.SelectedInteractable != null)
            if (_player.Interactor.SelectedInteractable.TryGetComponent(out Note note))
                AddNote(note.Collect());
    }

    private void OnEnable()
    {
        _player.Input.Player.Interact.performed += OverlapTrigger;
    }

    private void OnDisable()
    {
        _player.Input.Player.Interact.performed -= OverlapTrigger;
    }
}