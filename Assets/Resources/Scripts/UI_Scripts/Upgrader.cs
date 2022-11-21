using System;
using UnityEngine;
using UnityEngine.UI;

public class Upgrader : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite onSelectedSprite;
    [SerializeField] private GameObject upgrades;
    
    private int _previousSelected;
    private int _selected;

    private void Start()
    {
        for (var index = 0; index < buttons.Length; index++)
        {
            var button = buttons[index];

            var indexCopy = index;
            button.onClick.AddListener(() => { UpdateButton(indexCopy); });
        }
    }

    private void UpdateButton(int index)
    {
        _selected = index;

        buttons[_previousSelected].image.sprite = defaultSprite;
        buttons[index].image.sprite = onSelectedSprite;

        _previousSelected = _selected;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab)) upgrades.SetActive(true);
        else if(Input.GetKeyUp(KeyCode.Tab)) upgrades.SetActive(false);
    }
}