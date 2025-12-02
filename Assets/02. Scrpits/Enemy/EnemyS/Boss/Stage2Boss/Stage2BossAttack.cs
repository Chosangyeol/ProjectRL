using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2BossAttack : IAttackBehavior
{

    public bool isAttacking = false;

    public Stage2BossAttack()
    {

    }

    public void ExecuteAttack(EnemyBase enemy, int patternIndex = 0)
    {
        switch (patternIndex)
        {
            case 0:
                Debug.Log(patternIndex + 1 + "번 패턴");
                Pattern1(enemy);
                break;
            case 1:
                Debug.Log(patternIndex + 1 + "번 패턴");
                Pattern2(enemy);
                break;
            case 2:
                Debug.Log(patternIndex + 1 + "번 패턴");
                Pattern3(enemy);
                break;
            case 3:
                Debug.Log(patternIndex + 1 + "번 패턴");
                Pattern4(enemy);
                break;
        }
    }

    #region 패턴 1 - 전방 도끼 휘두르기
    public void Pattern1(EnemyBase enemy)
    {
        enemy.Anim.SetTrigger("Pattern1");
    }

    #endregion

    #region 패턴 2 - 도끼 회전
    public void Pattern2(EnemyBase enemy)
    {
        isAttacking = true;
        enemy.Anim.SetTrigger("Pattern2");
    }

    #endregion

    #region 패턴 3 - 지진
    public void Pattern3(EnemyBase enemy)
    {
        isAttacking = true;

        enemy.Anim.SetTrigger("Pattern3");
    }

    #endregion

    #region 패턴 4 - 돌진
    public void Pattern4(EnemyBase enemy)
    {
        isAttacking = true;

        enemy.Anim.SetTrigger("Pattern4");
        enemy.StartCoroutine(enemy.AttackDelay(5f));
    }

    #endregion
}
