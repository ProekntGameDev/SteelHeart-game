using UnityEngine;

public class Thorns : MonoBehaviour
{
    public float dameg;
    public bool ItThePit;
    //Parameter for adjusting the drop distance
    public float reclining;

    //Player Detection
    void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            other.GetComponent<Health>().TakeDamage(dameg);
            //if player down to pit
            if(ItThePit)
            {
                other.GetComponent<Transform>().position=new Vector3(PlayerPrefs.GetFloat("PosPlayerX"),PlayerPrefs.GetFloat("PosPlayerY"),PlayerPrefs.GetFloat("PosPlayerZ"));
            }
            //if thorns on level
            else
            {   
                //Search for a safe zone
                if(other.GetComponent<Transform>().position.x-transform.position.x>0)
                {
                    other.GetComponent<Transform>().position=new Vector3(other.GetComponent<Transform>().position.x+reclining,other.GetComponent<Transform>().position.y,other.GetComponent<Transform>().position.z);
                }
                else
                {
                    other.GetComponent<Transform>().position=new Vector3(other.GetComponent<Transform>().position.x-reclining,other.GetComponent<Transform>().position.y,other.GetComponent<Transform>().position.z);
                }
            }
        }

    }

}