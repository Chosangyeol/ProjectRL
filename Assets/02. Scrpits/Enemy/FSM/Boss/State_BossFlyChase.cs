using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_BossFlyChase : IState
{
    readonly EnemyBase enemy;
    readonly StateMachine fsm;
    readonly int patternCount;
    readonly int patternIndex;



    public State_BossFlyChase(EnemyBase enemy, StateMachine fsm, int patternCount, int patternIndex)
    {
        this.enemy = enemy;
        this.fsm = fsm;
        this.patternCount = patternCount;
        this.patternIndex = patternIndex;
    }

    public void OnEnter()
    {
        enemy.Anim.SetBool("isMoving", true);
    }

    public void Tick()
    {
        Vector3 enemyPos = enemy.transform.position;
        enemyPos.y = 0;

        float dist  = Vector3.Distance(enemyPos, enemy.player.position);

        RotateToPlayer(enemy, 5f);

        if (dist <= enemy.enemySO.attackRange)
        {
            if (IsFacingPlayer(enemy, 5f))
            {
                fsm.ChangeState(new State_BossAttack(enemy, fsm, patternCount, patternIndex));
            }
        }
        else
        {
            Vector3 target = enemy.player.position;
            target.y = enemy.flyHeight;

            Vector3 dir = (target - enemy.transform.position).normalized;
            enemy.gameObject.transform.position += dir * enemy.enemySO.moveSpeed * Time.deltaTime;
        }
    }

    public void FixedTick()
    {

    }

    public void OnExit()
    {
        enemy.Anim.SetBool("isMoving", false);
    }

    private void RotateToPlayer(EnemyBase enemy, float rotSpeed)
    {
        Vector3 dir = (enemy.player.position - enemy.transform.position);
        dir.y = 0;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRot, Time.deltaTime * rotSpeed);
    }

    private bool IsFacingPlayer(EnemyBase enemy, float thresholdAngle = 5f)
    {
        Vector3 dir = (enemy.player.position - enemy.transform.position).normalized;
        dir.y = 0;

        float angle = Vector3.Angle(enemy.transform.forward, dir);
        return angle < thresholdAngle;
    }

}
