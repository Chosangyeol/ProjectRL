using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase : EnemyBase
{
    [Header("보스 패턴")]
    public int patternCount;
    protected bool isSpecialPattern = false;

    [Header("다음 스테이지 포탈")]
    public GameObject portal;
    protected override void Awake()
    {
        base.Awake();
        if (portal != null )
            portal.SetActive(false);
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
            Debug.Log("추격 전환");
            fsm.ChangeState(new State_BossChase(this, fsm, patternCount));
        }
        Debug.Log("보스 공격 딜레이 종료");
    }

    protected override void Die()
    {
        if (portal != null)
            portal.SetActive(true);
    }

    protected void TryBossDrop(DropTableSO table)
    {
        if (table == null) return;

        for (int i = 0; i < table.rarityGroup[0].items.Count; i++)
        {
            PoolableMono dropItem = PoolManager.Instance.Pop(table.rarityGroup[0].items[i].item.name);
            if (!isFly)
            {
                Vector3 dropPos = this.gameObject.transform.position;
                dropPos.y = 0;
                dropPos += new Vector3(0, 1f, 0);
                dropPos.x += Random.Range(1, 5);
                dropPos.y += Random.Range(1, 5);

                dropItem.gameObject.transform.position = dropPos;
            }
            else if (isFly)
            {
                RaycastHit hit;
                if (Physics.Raycast(this.transform.position, Vector3.down, out hit, 100f, LayerMask.GetMask("Ground")))
                {
                    Vector3 dropPos = hit.point;
                    dropPos.x += Random.Range(1, 5);
                    dropPos.y += Random.Range(1, 5);
                    dropItem.gameObject.transform.position = dropPos;
                }
            }
        }
        Debug.Log("보스 아이템 드랍");
    }
}
