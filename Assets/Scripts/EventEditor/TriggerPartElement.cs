using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPartElement : MonoBehaviour
{
    UnityEngine.UI.Image stickerbox_image;
    Sprite default_sprite;
    GameObject next;
    //
    void Start()
    {
        UnityEngine.UI.Image[] images = gameObject.transform.GetComponentsInChildren<UnityEngine.UI.Image>();
        foreach (UnityEngine.UI.Image image in images) 
            if (image.gameObject.name == "empty_stickerbox") stickerbox_image = image;
        //
        default_sprite = stickerbox_image.sprite;
    }
    void Update()
    {
        if (stickerbox_image.sprite == default_sprite) return;
        if (next == null) next = Instantiate(Resources.Load("event_editor/EditorDropElementDefault") as GameObject, transform.position + (Vector3.down*60f)*EventUI.cv_transform.localScale.x, Quaternion.identity, transform.parent);
        //
             if (stickerbox_image.sprite.name == "condition");
        else if (stickerbox_image.sprite.name == "action") ;
    }
    //
    public void OpenV3Constructor() 
    {
        V3Constructor.s.SetActive(true);
        V3Constructor.s.transform.position = transform.position - Vector3.up * 25f * (transform.lossyScale.y + V3Constructor.s.transform.lossyScale.y);// + Vector3.right * (transform.lossyScale.x + V3Constructor.s.transform.lossyScale.x);
    }
}
