using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MineLaser : MonoBehaviour
{
    //public GameObject explosionPrefab; // Префаб эффекта взрыва
    [SerializeField] public float explosionRadius = 5f; // Радиус взрыва
    public LayerMask layerMask;
    public GameObject endPoint1;
    public GameObject endPoint2;


    // Компоненты линии
    LineRenderer line;
    RaycastHit hit;

    public void Start(){
        // Получаем компонент LineRenderer
        line = GetComponent<LineRenderer>();
    }

    public void Update(){
        

        // Отрисовываем линию
        line.SetPosition(0, endPoint1.transform.position);
        line.SetPosition(1, endPoint2.transform.position);

        // Проверяем, есть ли пересечение линии с другими объектами
        if (Physics.Linecast(endPoint1.transform.position, endPoint2.transform.position, out hit, layerMask))
        {
            Player player= hit.collider.GetComponent<Player>();
            if(player != null)
            {
                Explode();
            }
            
        }

         void Explode(){
            // Создаем эффект взрыва в текущей позиции объекта
           // GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
                hit.collider.GetComponent<Player>().Health.Kill();
            
            // Уничтожаем объект
            Destroy(gameObject);
        }
    }
}