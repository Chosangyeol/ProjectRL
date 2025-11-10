using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase : EnemyBase
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        // 보스전용 대기 FSM 세팅
        // 보스는 Patrol 없음
        // fsm.ChangeState(new State_Idle(this, fsm));
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
