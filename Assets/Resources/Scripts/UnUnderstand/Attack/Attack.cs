using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

     
    private float _timeAttack;
    public float startTimeAttack;

    public Transform attackPos;
    public LayerMask enemy;
    public float attackRange;
    public int damage;

    public Animator animator;

    private void Update()
    {
         animator.SetTrigger("Attack");
    
        if(_timeAttack <= 0){

         if(Input.GetKeyDown(KeyCode.Q))
         {
             Collider[] enemies = Physics.OverlapSphere(attackPos.position,attackRange,enemy);
             for ( int i =0; i< enemies.Length; i++)
             {
                  enemies[i].GetComponent< EnemyController>();
             }
         }
         _timeAttack=startTimeAttack;

        }
        else
        {
             _timeAttack -= Time.deltaTime;
        }

         void OnDrawGizmosSelected()
        {
             Gizmos.color = Color.red;
             Gizmos.DrawWireSphere(attackPos.position, attackRange);

        }


    }


}
