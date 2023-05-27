using UnityEngine;


public class MineLaser : MonoBehaviour
{

    //public GameObject explosionPrefab; // ������ ������� ������
    [SerializeField] public float explosionRadius = 5f; // ������ ������
    public LayerMask layerMask;
    public GameObject endPoint1;
    public GameObject endPoint2;


    private LineRenderer line;
    private RaycastHit hit;

    public void Start(){
        line = GetComponent<LineRenderer>();
    }


    public void Update(){
        

        // ������������ �����
        line.SetPosition(0, endPoint1.transform.position);
        line.SetPosition(1, endPoint2.transform.position);

        // ���������, ���� �� ����������� ����� � ������� ���������
        if (Physics.Linecast(endPoint1.transform.position, endPoint2.transform.position, out hit, layerMask))
        {
            Player player= hit.collider.GetComponent<Player>();
            if(player != null)
            {
                Explode();
            }
            
        }

         void Explode(){
            // ������� ������ ������ � ������� ������� �������
           // GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
                hit.collider.GetComponent<Player>().Health.Kill();
            
            // ���������� ������
            Destroy(gameObject);

        }

        Destroy(gameObject);
    }
}