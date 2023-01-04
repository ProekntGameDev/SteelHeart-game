using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MineLaser : MonoBehaviour
{
    //public GameObject explosionPrefab; // Префаб эффекта взрыва
    [SerializeField] public float explosionRadius = 5f; // Радиус взрыва

    // Компоненты линии
    LineRenderer line;
    RaycastHit hit;

    public void Start(){
        // Получаем компонент LineRenderer
        line = GetComponent<LineRenderer>();
    }

    public void Update(){
        // Определяем начальную и конечную точку линии
        Vector3 startPoint = transform.position;
        Vector3 endPoint = transform.forward * 50f; // Линия идет вперед от точки спауна

        // Отрисовываем линию
        line.SetPosition(0, startPoint);
        line.SetPosition(1, endPoint);

        // Проверяем, есть ли пересечение линии с другими объектами
        if (Physics.Linecast(startPoint, endPoint, out hit))
        {
            // Если пересечение обнаружено, взорвем мину
            Explode();
        }

         void Explode(){
            // Создаем эффект взрыва в текущей позиции объекта
           // GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);

            // Получаем все объекты в радиусе взрыва
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

            // Проходимся по всем найденным объектам и наносим им урон
            foreach (Collider nearbyObject in colliders)
            {
                Rigidbody rigidbody = nearbyObject.GetComponent<Rigidbody>();
                if (rigidbody != null)
                {
                    rigidbody.AddExplosionForce(100f, transform.position, explosionRadius);
                    if (hit.collider.CompareTag("Player"))
                    {
                        hit.collider.GetComponent<Player>().Health.Kill();
                    }
                }
            }

            // Уничтожаем объект
            Destroy(gameObject);
        }
    }
}