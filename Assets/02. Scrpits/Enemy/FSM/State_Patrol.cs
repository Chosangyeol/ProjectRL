using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class State_Patrol : IState
{
    readonly EnemyBase enemy;
    readonly StateMachine fsm;

    public State_Patrol(EnemyBase enemy, StateMachine fsm)
    {
        this.enemy = enemy;
        this.fsm = fsm;
    }

    public void OnEnter()
    {
        enemy.Anim.SetBool("isMoving", true);
        SetRandomDestination();
    }

    public void Tick()
    {
        if (!enemy.Agent.hasPath || enemy.Agent.remainingDistance < 0.5f)
            fsm.ChangeState(new State_Idle(enemy, fsm));

        float dist = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);
        if (dist <= enemy.enemySO.detectRange)
        {
            fsm.ChangeState(new State_Chase(enemy, fsm));
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

}
