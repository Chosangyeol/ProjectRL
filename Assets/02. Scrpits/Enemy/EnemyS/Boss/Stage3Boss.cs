using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3Boss : BossBase
{
    public GameObject pattern2Centor;
    public GameObject pattern2Projectile;

    private void Start()
    {
        attackBehavior = new Stage3BossAttack(pattern2Centor, pattern2Projectile);

        lr = GetComponent<LineRenderer>();
        lr.enabled = false;
    }

    private void Update()
    {
        
        fsm.Tick();
    }
}
