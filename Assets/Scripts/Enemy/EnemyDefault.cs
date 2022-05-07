using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDefault : MonoBehaviour
{
    [Header("Хар-ки врага")]
    [SerializeField] private float _enemyHealth; //HP
    [SerializeField] private float _collisionDamage; 


    void Start()
    {
        
    }


    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player") 
        {
            PlayerMovment Player = collision.gameObject.GetComponent<PlayerMovment>(); //ссылка на игрока
            Player.TakeDamage(_collisionDamage); //нанесение урона игроку
        }
    }
}
