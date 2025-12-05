using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class State_FlyChase : IState
{
    readonly EnemyBase enemy;
    readonly StateMachine fsm;
    private float hoverTimer;

    public State_FlyChase(EnemyBase enemy, StateMachine fsm)
    {
        this.enemy = enemy;
        this.fsm = fsm;
    }

    public void OnEnter()
    {
        enemy.Anim.SetBool("Chase", true);
    }

    public void Tick()
    {
        MoveTo(enemy.player.position);

        Vector3 enemyPos = enemy.transform.position;

        if (Physics.Raycast(enemy.transform.position, Vector3.down, out RaycastHit hit, 100f))
        {
            enemyPos.y = hit.point.y;
        }
        else
        {
            enemyPos.y = 0;
        }
        float dist = Vector3.Distance(enemy.player.transform.position, enemyPos);

        if (dist <= enemy.enemySO.attackRange)
        {
            if (IsFacingPlayer(enemy, 5f))
            {
                fsm.ChangeState(new State_Attack(enemy, fsm));
            }
        }
    }

    public void FixedTick()
    {

    }

    public void OnExit()
    {
        enemy.Anim.SetBool("Chase", false);
    }

    public void MoveTo(Vector3 targetPos)
    {
        Vector3 adjusted = GetHoverPosition(targetPos);
        Vector3 dir = (adjusted - enemy.transform.position).normalized;

        enemy.transform.position += dir * enemy.GetStat().moveSpeed * Time.deltaTime;
        enemy.transform.forward = Vector3.Lerp(enemy.transform.forward, dir, 10f * Time.deltaTime);
    }

    private Vector3 GetHoverPosition(Vector3 target)
    {
        // 1) 지면 감지
        if (Physics.Raycast(enemy.transform.position, Vector3.down, out RaycastHit hit, 100f))
        {
            target.y = hit.point.y + enemy.flyHeight;
        }

        // 2) 둥둥 떠다니는 효과
        hoverTimer += Time.deltaTime * 2;
        target.y += Mathf.Sin(hoverTimer) * enemy.hover;

        return target;
    }

    private bool IsFacingPlayer(EnemyBase enemy, float thresholdAngle = 5f)
    {
        Vector3 dir = (enemy.player.position - enemy.transform.position).normalized;
        dir.y = 0;

        float angle = Vector3.Angle(enemy.transform.forward, dir);
        return angle < thresholdAngle;
    }
}
