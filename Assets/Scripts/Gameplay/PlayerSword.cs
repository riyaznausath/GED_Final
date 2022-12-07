using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    bool isAttacking = false;

    public void OnAttack()
    {
        isAttacking = true;
    }

    public void OnTriggerEnter(Collider col)
    {
       if(col.tag == "Enemy" && isAttacking == true)
        {
            EnemyController enemy = col.GetComponent<EnemyController>();
            enemy.DestroyEnemy();
            isAttacking = false;
        }
    }
}
