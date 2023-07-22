using UnityEngine;

public class Note : MonoBehaviour
{
    [SerializeField] private NoteData _data;

    private void Awake()
    {
        if (_data == null)
            throw new System.NullReferenceException(nameof(_data));
    }

    public NoteData Collect()
    {
        Destroy(gameObject);
        return _data;
    }
}
