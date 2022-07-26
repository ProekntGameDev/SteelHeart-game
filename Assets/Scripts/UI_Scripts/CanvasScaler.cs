using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScaler : MonoBehaviour
{
    void Start()
    {
        GameObject.Find("Canvas").GetComponent<Canvas>().scaleFactor = 1920f / Screen.currentResolution.width;
    }
}
