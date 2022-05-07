using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SteelHeart
{
    public class EnemyDefault : Unit
    {
        [Header("���-�� �����")] [SerializeField]
        private float _enemyHealth; //HP

        [SerializeField] private float _collisionDamage;

        private const string PlayerTag = "Player";
        
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag(PlayerTag))
            {
                var player = collision.gameObject.GetComponent<Player>(); //������ �� ������
                player.TakeDamage(_collisionDamage); //��������� ����� ������
            }
        }
    }
}
