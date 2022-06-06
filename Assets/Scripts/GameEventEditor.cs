using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventEditor : MonoBehaviour
{
    GameObject drop_element_prefab;
    GameObject[] drop_elements_pool;
    UnityEngine.UI.Text[] drop_elements_text;
    int pool_lenght = 8;
    string[] choices;

    void Start()
    {
        drop_element_prefab = (GameObject)Resources.Load("event_editor/EditorDropElement");
        drop_elements_pool = new GameObject[pool_lenght];
        drop_elements_text = new UnityEngine.UI.Text[pool_lenght];
        for (int i = 0; i < pool_lenght; ++i) drop_elements_pool[i] =
        Instantiate(drop_element_prefab, transform.position - Vector3.right * 128 - Vector3.up * (96f + i * 64f), Quaternion.identity);
        for (int i = 0; i < pool_lenght; ++i) drop_elements_text[i] = drop_elements_pool[i].GetComponent<UnityEngine.UI.Text>();
    }
    void Update()
    {
        for (int i = 0; i < pool_lenght; ++i) { drop_elements_pool[i].SetActive(i < choices.Length && choices[i] != null); }
        drop_elements_text[1].
    }
}
