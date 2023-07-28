using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using NaughtyAttributes;

[RequireComponent(typeof(SphereCollider))]
public class Journal : MonoBehaviour
{
    public UnityEvent<NoteData> OnNoteAdded;

    public IReadOnlyList<NoteData> Notes => _notes;

    [SerializeField, Required] private Player _player;

    private SphereCollider _pickupRange;
    private List<NoteData> _notes = new List<NoteData>();

    private void Start()
    {
        _pickupRange = GetComponent<SphereCollider>();
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
        Collider[] colliders = Physics.OverlapSphere(_pickupRange.transform.position + _pickupRange.center, _pickupRange.radius,
            Physics.AllLayers, QueryTriggerInteraction.Ignore);

        foreach (var other in colliders)
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
        if (_player.Input == null)
            return;

        _player.Input.Player.Interact.performed -= (c) => OverlapTrigger();
    }
}