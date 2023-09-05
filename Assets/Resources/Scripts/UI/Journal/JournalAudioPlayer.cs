using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class JournalAudioPlayer : MonoBehaviour
{
    public bool IsPlaying => _audioSource != null && _audioSource.isPlaying;

    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _currentTime;
    [SerializeField] private TextMeshProUGUI _audioLength;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _pauseButton;

    private AudioSource _audioSource;
    private float _lastTime;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _slider.onValueChanged.AddListener(OnSetTime);

        _resumeButton.onClick.AddListener(Play);
        _pauseButton.onClick.AddListener(Pause);
    }

    public void UpdateAudio(AudioClip clip)
    {
        if (IsPlaying)
            _audioSource.Stop();

        _lastTime = 0;

        _audioSource.clip = clip;
        _slider.maxValue = clip.length;

        _audioLength.text = $"{(int)clip.length / 60}.{(int)clip.length % 60}";
        UpdateUI(0);
    }

    public void Play()
    {
        if (_audioSource.isPlaying)
            return;

        PlayAtTime(_lastTime);

        _resumeButton.gameObject.SetActive(false);
        _pauseButton.gameObject.SetActive(true);
    }

    public void Pause()
    {
        if (_audioSource.clip == null)
            return;

        _lastTime = _audioSource.time;

        _audioSource.Stop();

        _resumeButton.gameObject.SetActive(true);
        _pauseButton.gameObject.SetActive(false);
    }

    public void ToEnd()
    {
        if (_audioSource.clip == null)
            return;

        Pause();
        _lastTime = _audioSource.clip.length;
        UpdateUI(_audioSource.clip.length);
    }

    public void ToStart()
    {
        Pause();
        _lastTime = 0;
        UpdateUI(0);
    }

    private void OnSetTime(float time)
    {
        if(IsPlaying)
            Pause();

        _lastTime = time;
        UpdateUI(time);
    }

    private void PlayAtTime(float time)
    {
        _audioSource.time = time;
        _audioSource.Play();
    }

    private void Update()
    {
        if (_audioSource.clip == null)
            return;

        if (_audioSource.time >= _audioSource.clip.length)
        {
            Pause();
            _lastTime = 0;
            UpdateUI(_audioSource.time);
        }

        if(IsPlaying)
            UpdateUI(_audioSource.time);
    }

    private void UpdateUI(float time)
    {
        _slider.SetValueWithoutNotify(time);
        _currentTime.text = $"{(int)time / 60}.{(int)time % 60}";
    }
}
