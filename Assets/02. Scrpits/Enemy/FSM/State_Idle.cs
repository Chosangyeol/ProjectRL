using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Idle : IState
{
    readonly EnemyBase enemy;
    readonly StateMachine fsm;
    private float timer = 0;

    public State_Idle(EnemyBase enemy, StateMachine fsm)
    {
        this.enemy = enemy;
        this.fsm = fsm;
    }

    public void OnEnter()
    {
        if (!enemy.isFly)
            enemy.Agent.ResetPath();

        enemy.Anim.SetTrigger("Idle");
    }

    public void Tick()
    {
        timer += Time.deltaTime;

        if (timer >= 3f)
        {
            fsm.ChangeState(new  State_Patrol(enemy, fsm));
        }

        if (Vector3.Distance(enemy.player.position, enemy.transform.position) <= enemy.enemySO.detectRange)
        {
            fsm.ChangeState(new State_Chase(enemy, fsm));
        }
    }

    public void FixedTick() { }

    public void OnExit() { }
}
