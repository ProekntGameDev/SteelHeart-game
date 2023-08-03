using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using NaughtyAttributes;

[RequireComponent(typeof(OverlapSphere))]
public class Journal : MonoBehaviour
{
    public UnityEvent<NoteData> OnNoteAdded;

    public IReadOnlyList<NoteData> Notes => _notes;

    [SerializeField, Required] private Player _player;

    private OverlapSphere _pickupRange;
    private List<NoteData> _notes = new List<NoteData>();

    private void Start()
    {
        _pickupRange = GetComponent<OverlapSphere>();
    }

    public void AddNote(NoteData noteData)
    {
        if (noteData == null)
            throw new System.ArgumentNullException(nameof(noteData));

        _notes.Add(noteData);
        OnNoteAdded?.Invoke(noteData);
    }

    private void OverlapTrigger()
    {
        foreach (var other in _pickupRange.GetColliders())
        {
            if (other.TryGetComponent(out Note note))
                AddNote(note.Collect());
        }
    }

    private void OnEnable()
    {
        _player.Input.Player.Interact.performed += (c) => OverlapTrigger();
    }

    private void OnDisable()
    {
        _player.Input.Player.Interact.performed -= (c) => OverlapTrigger();
    }
}