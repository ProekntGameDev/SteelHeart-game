using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V3Constructor : MonoBehaviour
{
    public static GameObject s;
    private void Awake()
    {
        s = gameObject;
    }
    public void Close()
    {
        s.SetActive(false);
    }
}
