using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_BossAttack : IState
{
    readonly EnemyBase enemy;
    readonly StateMachine fsm;
    readonly int patternCount;
    readonly int nowPatternIndex;
    public State_BossAttack(EnemyBase enemy, StateMachine fsm, int patternCount, int nowPatternIndex)
    {
        this.enemy = enemy;
        this.fsm = fsm;
        this.patternCount = patternCount;
        this.nowPatternIndex = nowPatternIndex;
    }

    public void OnEnter()
    {
        enemy.Agent.ResetPath();
        enemy.Anim.SetBool("isMoving",false);
        enemy.StartAttack(nowPatternIndex);
    }

    public void Tick()
    {
        
    }

    public void FixedTick()
    {

    }

    public void OnExit()
    {

    }
}
