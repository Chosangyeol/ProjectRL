using Info;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Melee1 : EnemyBase
{
    public float rushSpeed;
    protected override void Start()
    {
        base.Start();

        attackBehavior = new Stage2Melee1Attack(rushSpeed);
    }
}
