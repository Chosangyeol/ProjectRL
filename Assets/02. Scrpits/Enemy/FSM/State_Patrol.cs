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
        enemy.anim.SetBool("isPatrol", true);

    }

    public void Tick()
    {
        if (!enemy.agent.hasPath || enemy.agent.remainingDistance < 0.5f)
        { }

        float dist = Vector3.Distance(enemy.transform.position, enemy.player.position);
        if (dist <= enemy.enemySO.detectRange)
        {
            fsm.ChangeState(new State_Chase(enemy, fsm));
        }
    }

    public void FixedTick() { }

    public void OnExit() { }

    private void SetRandomDestination()
    {
        Vector3 randomPos = Random.insideUnitSphere * enemy.enemySO.patrolRange;
        randomPos += enemy.transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPos, out hit, enemy.enemySO.patrolRange, 1);
        enemy.agent.SetDestination(hit.position);
    }
}
