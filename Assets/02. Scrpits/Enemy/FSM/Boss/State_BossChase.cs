using UnityEngine;

public class State_BossChase : IState
{
    readonly EnemyBase enemy;
    readonly StateMachine fsm;
    readonly int patternCount;
    public int patternIndex;

    public State_BossChase(EnemyBase enemy, StateMachine fsm, int patternCount)
    {
        this.enemy = enemy;
        this.fsm = fsm;
        this.patternCount = patternCount;
    }

    public void OnEnter()
    {
        patternIndex = Random.Range(0, patternCount);
        enemy.Anim.SetBool("isMoving", true);
    }

    public void Tick()
    {
        if (enemy.isFixedType)
        {
            float dist = Vector3.Distance(enemy.transform.position, enemy.player.position);

            RotateToPlayer(enemy, 5f);

            if (dist <= enemy.enemySO.attackRange)
            {
                if (IsFacingPlayer(enemy, 5f))  // ★ 방향 완전히 맞으면
                {
                    fsm.ChangeState(new State_BossAttack(enemy, fsm, patternCount, patternIndex));
                }
            }
        }
        else
        {

            // 자연스럽게 플레이어 방향으로 돌기
            float dist = Vector3.Distance(enemy.transform.position, enemy.player.position);

            RotateToPlayer(enemy, 5f);

            if (dist <= enemy.enemySO.attackRange)
            {
                if (IsFacingPlayer(enemy, 5f))  // ★ 방향 완전히 맞으면
                {
                    fsm.ChangeState(new State_BossAttack(enemy, fsm, patternCount, patternIndex));
                }
            }
            else
            {
                enemy.Agent.SetDestination(enemy.player.position);
            }
            Debug.Log("Chase");
        }
    }

    public void FixedTick() { }
    public void OnExit()
    {
        enemy.Agent.ResetPath();
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
