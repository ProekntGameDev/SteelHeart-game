using UnityEngine;
using Interfaces;

public class Note : MonoBehaviour, ITriggerableMonoBehaviour
{
    public NoteData data;


    private void Awake()
    {
        if (data == null)
            Debug.LogError($"Заметка {gameObject.name} не имеет данных");
    }

    public void Trigger(Transform obj)
    {
        if (obj.GetComponent<PlayerMovement>() == null) return;

        if (StaticGameData.AddNote(data))
            Debug.Log("Note: " + data.title + "\n" + "Text: " + data.text);
        else
            Debug.LogError("Эта заметка уже была добавлена! " +
                "Проверь, чтобы не было дубликатов (если только это не было сделано специально)");
        gameObject.SetActive(false);
    }
}
