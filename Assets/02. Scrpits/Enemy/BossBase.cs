using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase : EnemyBase
{
    public int patternCount;
    protected bool isSpecialPattern = false;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        // 보스전용 대기 FSM 세팅
        // 보스는 Patrol 없음
        Reset();
        fsm.ChangeState(new State_BossIdle(this, fsm, patternCount));
    }

    public override void StartAttack(int patternIndex = 0)
    {
        attackBehavior.ExecuteAttack(this, patternIndex);
    }

    public override IEnumerator AttackDelay(float delay)
    {
        Debug.Log("보스 공격 딜레이 시작");
        yield return new WaitForSeconds(delay);
        if (!isSpecialPattern)
        {
            fsm.ChangeState(new State_BossChase(this, fsm, patternCount));
        }
        Debug.Log("보스 공격 딜레이 종료");
    }

    protected override void Die()
    {
        // 보스 전용 아이템 드랍 함수
        TryBossDrop(enemySO.itemDropTable);
    }

    private void TryBossDrop(DropTableSO dropTable)
    {
        Debug.Log("보스 아이템 드랍");
    }
}
