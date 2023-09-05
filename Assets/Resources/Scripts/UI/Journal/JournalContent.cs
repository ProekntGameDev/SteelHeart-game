using UnityEngine;
using TMPro;

public class JournalContent : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _type;
    [SerializeField] private TextMeshProUGUI _header;
    [SerializeField] private TextMeshProUGUI _subheader;
    [SerializeField] private JournalAudioPlayer _audioPlayer;
    [SerializeField] private GameObject _audioContentPanel;
    [SerializeField] private TextMeshProUGUI _audioContent;
    [SerializeField] private GameObject _textContentPanel;
    [SerializeField] private TextMeshProUGUI _content;

    private void Start()
    {
        _audioContentPanel.SetActive(false);
        _textContentPanel.SetActive(false);
    }

    public void UpdateContent(NoteData noteData, string typeName)
    {
        _audioPlayer.Pause();

        _type.text = typeName;
        _header.text = noteData.Header;
        _subheader.text = noteData.Subheader;

        if (noteData.NoteContent.Audio == null)
        {
            _textContentPanel.SetActive(true);
            _audioContentPanel.SetActive(false);
            _content.text = noteData.NoteContent.Text;
        }
        else
        {
            _audioContentPanel.SetActive(true);
            _textContentPanel.SetActive(false);
            _audioPlayer.UpdateAudio(noteData.NoteContent.Audio);
            _audioContent.text = noteData.NoteContent.Text;
        }
    }
}
