using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_BossIdle : IState
{
    readonly EnemyBase enemy;
    readonly StateMachine fsm;

    public State_BossIdle(EnemyBase enemy, StateMachine fsm)
    {
        this.enemy = enemy;
        this.fsm = fsm;
    }

    public void OnEnter()
    {
        enemy.Anim.SetBool("isBossIdle", true);
    }

    public void Tick()
    {
        float dist = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);

        if (dist <= enemy.enemySO.attackRange)
        {
            // АјАн
        }
        else if (dist <= enemy.enemySO.detectRange)
        {

        }
        Debug.Log("BossIdle");
    }

    public void FixedTick()
    {

    }

    public void OnExit()
    {

    }
}
