using UnityEngine;
using TMPro;

public class JournalContent : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _header;
    [SerializeField] private TextMeshProUGUI _subheader;
    [SerializeField] private TextMeshProUGUI _content;

    public void UpdateContent(NoteData noteData)
    {
        _header.text = noteData.Header;
        _subheader.text = noteData.Subheader;
        _content.text = noteData.NoteContent.Text;
    }
}
