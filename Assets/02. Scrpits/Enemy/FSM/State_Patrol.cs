using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class State_Patrol : IState
{
    readonly EnemyBase enemy;
    readonly StateMachine fsm;
    private float hoverTimer;

    private float patrolTimer = 0;

    private Vector3 patrolTarget;
    private bool hasPatrolTarget;

    public State_Patrol(EnemyBase enemy, StateMachine fsm)
    {
        this.enemy = enemy;
        this.fsm = fsm;
    }

    public void OnEnter()
    {
        enemy.Anim.SetBool("isMoving", true);
        if (enemy.isFly)
        {
            Vector2 circle = Random.insideUnitCircle * enemy.enemySO.patrolRange;
            patrolTarget = enemy.transform.position + new Vector3(circle.x, 0, circle.y);
            hasPatrolTarget = true;
            return;
        }

        SetRandomDestination();     
    }

    public void Tick()
    {
        if (!enemy.isFly)
        {
            if (!enemy.Agent.hasPath || enemy.Agent.remainingDistance < 0.5f)
                fsm.ChangeState(new State_Idle(enemy, fsm));
        }

        if (enemy.isFly)
        {
            MoveTo(patrolTarget);

            if(Vector3.Distance(enemy.transform.position, patrolTarget) < 1f)
            {
                Vector2 circle = Random.insideUnitCircle * enemy.enemySO.patrolRange;
                patrolTarget = enemy.transform.position + new Vector3(circle.x, 0, circle.y);
            }
        }

        if (IsPlayerInSight())
        {
            if (!enemy.isFly)
                fsm.ChangeState(new State_Chase(enemy, fsm));
            else
                fsm.ChangeState(new State_FlyChase(enemy, fsm));
        }

        patrolTimer += Time.deltaTime;
        if (patrolTimer >= 10f)
        {
            patrolTimer = 0;
            fsm.ChangeState(new State_Idle(enemy, fsm));
        }
    }

    public void FixedTick() { }

    public void OnExit()
    {
        enemy.Anim.SetBool("isMoving", false);
    }
    private void SetRandomDestination()
    {
        const int maxAttempts = 20; // 최대 재시도 횟수
        Vector3 result = enemy.transform.position;

        for (int i = 0; i < maxAttempts; i++)
        {
            // 구 범위 안에서 랜덤 위치 생성
            Vector3 randomPos = Random.insideUnitSphere * enemy.enemySO.patrolRange;
            randomPos += enemy.transform.position;

            // NavMesh 위의 점 샘플링
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPos, out hit, enemy.enemySO.patrolRange, NavMesh.AllAreas))
            {
                result = hit.position;
                break;
            }
        }

        enemy.Agent.SetDestination(result);
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

    private bool IsPlayerInSight()
    {
        Vector3 dirToPlayer = enemy.player.transform.position - enemy.transform.position;
        float distance = dirToPlayer.magnitude;

        // 1) 탐지 거리 체크
        if (distance > enemy.enemySO.detectRange)
            return false;

        // 2) 시야 각도 체크 (전방 기준)
        float angle = Vector3.Angle(enemy.transform.forward, dirToPlayer);

        if (angle > enemy.enemySO.detectAngle * 0.5f)
            return false;

        // 3) 장애물 체크 (옵션)
        if (Physics.Raycast(enemy.transform.position + Vector3.up * 1f, dirToPlayer.normalized, out RaycastHit hit, distance))
        {
            if (!hit.collider.CompareTag("Player"))
            {
                return false; // 벽 등에 가려진 경우
            }
        }

        return true;
    }

}
