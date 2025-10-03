using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy2 : EnemyBase
{
    public GameObject projectile;
    public Transform firePos;
    void Start()
    {
        attackBehavior = new RangedAttack(projectile, firePos);
    }

}
