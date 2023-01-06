using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour, IPickupable
{
    
    public GameObject pickedUpObject;
     public bool isPickedUp;

    // Реализация метода Pickup интерфейса IPickupable
    public void Pickup()
    {
        // Объект взят
        isPickedUp = true;
        // Отключаем коллайдер, чтобы объект не мешался
        pickedUpObject.GetComponent<Collider>().enabled = false;
        // Отключаем физику, чтобы объект можно было перемещать вручную
        pickedUpObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    // Реализация метода Drop интерфейса IPickupable
    public void Drop()
    {
        if (isPickedUp)
        {
            // Включаем коллайдер и физику
            pickedUpObject.GetComponent<Collider>().enabled = true;
            pickedUpObject.GetComponent<Rigidbody>().isKinematic = false;
            // Добавляем силу впереди игрока
            pickedUpObject.GetComponent<Rigidbody>().AddForce(transform.up * 10);
        }
    }
}
