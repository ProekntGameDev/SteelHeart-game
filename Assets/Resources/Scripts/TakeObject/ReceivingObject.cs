using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceivingObject : MonoBehaviour
{
    public bool Received { get; private set; } = false;
    public void ReceivedItem() => Received = true;
}
