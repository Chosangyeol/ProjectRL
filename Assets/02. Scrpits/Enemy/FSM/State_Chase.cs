using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Chase : IState
{
    readonly EnemyBase enemy;
    readonly StateMachine fsm;

    public State_Chase(EnemyBase enemy, StateMachine fsm)
    {
        this.enemy = enemy;
        this.fsm = fsm;
    }

    public void OnEnter()
    {
        enemy.anim.SetBool("isMoving", true);
    }

    public void Tick()
    {
        float dist = Vector3.Distance(enemy.transform.position, enemy.player.position);

        if (dist > enemy.enemySO.detectRange * 1.2f)
        {
            fsm.ChangeState(new State_Patrol(enemy, fsm));
        }
        else if (dist <= enemy.enemySO.attackRange)
        {
            fsm.ChangeState(new State_Attack(enemy, fsm));
        }
        else
        {
            enemy.agent.SetDestination(enemy.player.position);
        }
    }

    public void FixedTick() { }
    public void OnExit()
    {
        enemy.agent.ResetPath();
    }
}
