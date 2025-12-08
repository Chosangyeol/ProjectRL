using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State_Die : IState
{
    readonly EnemyBase enemy;
    readonly StateMachine fsm;

    public State_Die(EnemyBase enemy, StateMachine fsm)
    {
        this.enemy = enemy;
        this.fsm = fsm;
    }

    public void OnEnter()
    {
        if (enemy.Agent != null)
            enemy.Agent.ResetPath();
    }

    public void Tick()
    {

    }

    public void FixedTick() { }

    public void OnExit()
    {
    }


}
