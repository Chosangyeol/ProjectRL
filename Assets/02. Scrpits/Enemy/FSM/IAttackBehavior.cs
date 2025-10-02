using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackBehavior
{
    void ExecuteAttack(EnemyBase enemy);
}

public class MeleeAttack : IAttackBehavior
{
    public void ExecuteAttack(EnemyBase enemy)
    {
        enemy.anim.SetTrigger("Attack");
    }
}