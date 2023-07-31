using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class JournalNoteUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _header;
    [SerializeField] private TextMeshProUGUI _type;
    [SerializeField] private Button _button;

    private JournalContent _journalContent;
    private NoteData _noteData;

    public void Init(JournalContent journalContent, NoteData noteData)
    {
        _journalContent = journalContent;
        _header.text = noteData.Header;
        _noteData = noteData;
    }

    private void Start()
    {
        _button.onClick.AddListener(Select);
    }

    private void Select()
    {
        _journalContent.UpdateContent(_noteData, _type.text);
    }
}
