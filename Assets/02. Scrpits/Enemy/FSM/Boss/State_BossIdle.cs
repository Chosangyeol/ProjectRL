using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_BossIdle : IState
{
    readonly EnemyBase enemy;
    readonly StateMachine fsm;
    readonly int patternCount;

    public State_BossIdle(EnemyBase enemy, StateMachine fsm, int patternCount)
    {
        this.enemy = enemy;
        this.fsm = fsm;
        this.patternCount = patternCount;
    }

    public void OnEnter()
    {
        enemy.Anim.SetBool("isBossIdle", true);
    }

    public void Tick()
    {
        float dist = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);

        if (dist <= enemy.enemySO.detectRange)
        {
            fsm.ChangeState(new State_BossChase(enemy, fsm, patternCount));
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
