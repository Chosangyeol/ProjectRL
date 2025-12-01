using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : EnemyBase
{
    private void Start()
    {
        attackBehavior = new MeleeAttack();
    }
}
