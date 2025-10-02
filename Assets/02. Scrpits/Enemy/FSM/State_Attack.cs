using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Attack : IState
{
    readonly EnemyBase enemy;
    readonly StateMachine fsm;

    public State_Attack(EnemyBase enemy, StateMachine fsm)
    {
        this.enemy = enemy;
        this.fsm = fsm;
    }

    public void OnEnter()
    {
        enemy.agent.ResetPath();
        enemy.anim.SetBool("isMoving", false);
    }

    public void Tick()
    {
        float dist = Vector3.Distance(enemy.transform.position, enemy.player.position);

        if (dist > enemy.enemySO.attackRange)
        {
            fsm.ChangeState(new State_Chase(enemy, fsm));
            return;
        }

        enemy.StartAttack();
    }

    public void FixedTick() { }

    public void OnExit() { }
}
