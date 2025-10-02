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
        enemy.anim.SetBool("isMoving", true);
        SetRandomDestination();
    }

    public void Tick()
    {
        if (!enemy.agent.hasPath || enemy.agent.remainingDistance < 0.5f)
            SetRandomDestination();

        float dist = Vector3.Distance(enemy.transform.position, enemy.player.position);
        if (dist <= enemy.enemySO.detectRange)
        {
            fsm.ChangeState(new State_Chase(enemy, fsm));
        }
        Debug.Log("Patrol");
    }

    public void FixedTick() { }

    public void OnExit() { }

    private void SetRandomDestination()
    {
        Vector3 randomPos = Random.insideUnitSphere * 100f;
        randomPos += enemy.transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPos, out hit, enemy.enemySO.patrolRange, 1);
        enemy.agent.SetDestination(hit.position);
    }
}
