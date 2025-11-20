using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy2 : EnemyBase
{
    public GameObject projectile;
    public Transform firePos;
    public float attackPreDelay;

    protected override void Awake()
    {
        base.Awake();
        lr = GetComponent<LineRenderer>();

    }

    protected override void Start()
    {
        base.Start();

        attackBehavior = new RangedAttack(projectile, firePos, attackPreDelay);
    }

}
