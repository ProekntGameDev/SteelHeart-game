using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropElement : MonoBehaviour
{
    PointerEventData ptr_e_data;
    //
    static GameObject s_dublicate;
    static Vector3 half_res = new Vector3(Screen.width/2, Screen.height/2);
    void Update()
    {
        bool isDublicateExists = s_dublicate != null;
        if (isDublicateExists && (gameObject != s_dublicate)) return;
        //
        bool isPressed = Input.GetMouseButton(0);
        if (isDublicateExists)
        {
            if (isPressed)
            {
                s_dublicate.transform.position = EventUI.cv_transform.position + (Input.mousePosition - half_res)*EventUI.cv_transform.localScale.x;
                return;
            }
            GameObject box = isHit("empty_stickerbox");
            if (box != null) box.GetComponent<UnityEngine.UI.Image>().sprite = gameObject.GetComponent<UnityEngine.UI.Image>().sprite;
            DestroyImmediate(s_dublicate);
            s_dublicate = null;
        }
        //destroy or move copy^
        else if (isHit(gameObject) && isPressed) s_dublicate = Instantiate(gameObject, EventUI.cv_transform);
        //create copy^
    }

    bool isHit(GameObject obj) 
    {
        ptr_e_data = new PointerEventData(EventSystem.current);
        ptr_e_data.position = Input.mousePosition;
        List<RaycastResult> raycast_result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(ptr_e_data, raycast_result);
        foreach (RaycastResult hit in raycast_result)
        {
            bool isHit = hit.gameObject != null && hit.gameObject == obj;
            if (isHit) return true;
        }
        return false;
    }

    GameObject isHit(string name)
    {
        ptr_e_data = new PointerEventData(EventSystem.current);
        ptr_e_data.position = Input.mousePosition;
        List<RaycastResult> raycast_result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(ptr_e_data, raycast_result);
        foreach (RaycastResult hit in raycast_result)
        {
            if (hit.gameObject != null && hit.gameObject.name == name) return hit.gameObject;
        }
        return null;
    }
}
