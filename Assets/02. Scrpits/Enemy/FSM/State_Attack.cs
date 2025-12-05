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
        if (enemy.Agent != null)
            enemy.Agent.ResetPath();
        enemy.Anim.SetBool("isMoving", false);
        enemy.Anim.SetBool("Chase", false);
        enemy.StartAttack();
    }

    public void Tick()
    { 

    }

    public void FixedTick() { }

    public void OnExit() { }
}
