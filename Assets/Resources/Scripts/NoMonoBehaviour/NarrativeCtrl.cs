using UnityEngine;
using UnityEngine.UI;


public class NarrativeCtrl : MonoBehaviour
{
    public string noteTextstr;
    public GameObject notice;
    public GameObject noteUI;
    public Text text;

    private void Method1()
    {
        text.text = noteTextstr;
        if (Input.GetKey(KeyCode.E))
        {
            noteUI.SetActive(true);
        }

        if (Input.GetKey(KeyCode.T))
        {
            noteUI.SetActive(false);
        }

        notice.SetActive(true);
    }

    private void Method2()
    {
        notice.SetActive(false);
        noteUI.SetActive(false);
    }
}
