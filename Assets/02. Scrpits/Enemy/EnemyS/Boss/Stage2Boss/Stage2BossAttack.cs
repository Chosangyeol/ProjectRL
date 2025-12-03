using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stage2BossAttack : IAttackBehavior
{

    public bool isAttacking = false;
    public GameObject pattern3Warning;
    public GameObject pattern3Effect;
    public GameObject pattern4Effect;
    public Stage2BossAttack(GameObject pattern3Warning, GameObject pattern3Effect,GameObject pattern4Effect)
    {
        this.pattern3Warning = pattern3Warning;
        this.pattern3Effect = pattern3Effect;
        this.pattern4Effect = pattern4Effect;
    }

    public void ExecuteAttack(EnemyBase enemy, int patternIndex = 0)
    {
        if (enemy is Stage2Boss boss)
        {
            if (Vector3.Distance(enemy.player.position, enemy.transform.position) >= 30f && boss.canRush)
            {
                enemy.StartCoroutine(Pattern4(enemy));
                return;
            }

            switch (patternIndex)
            {
                case 0:
                    Debug.Log(patternIndex + 1 + "번 패턴");
                    enemy.StartCoroutine(Pattern1(enemy));
                    break;
                case 1:
                    Debug.Log(patternIndex + 1 + "번 패턴");
                    enemy.StartCoroutine(Pattern2(enemy));

                    break;
                case 2:
                    Debug.Log(patternIndex + 1 + "번 패턴");
                    enemy.StartCoroutine(Pattern3(enemy));
                    break;
            }
        }     
    }

    #region 패턴 1 - 전방 도끼 휘두르기
    IEnumerator Pattern1(EnemyBase enemy)
    {
        isAttacking = true;

        enemy.Anim.SetBool("isMoving", true);
        while (Vector3.Distance(enemy.player.position, enemy.transform.position) >= 7f)
        {
            RotateToPlayer(enemy, 5f);
            enemy.Agent.SetDestination(enemy.player.position);
            yield return null;
        }
        enemy.Agent.ResetPath();
        enemy.Agent.velocity = Vector3.zero;

        enemy.Anim.SetBool("isMoving", false);
        enemy.Anim.SetTrigger("Pattern1");
    }

    #endregion

    #region 패턴 2 - 도끼 회전
    IEnumerator Pattern2(EnemyBase enemy)
    {
        isAttacking = true;
        enemy.Anim.SetBool("isMoving", true);
        while (Vector3.Distance(enemy.player.position, enemy.transform.position) >= 8f)
        {
            RotateToPlayer(enemy, 5f);
            enemy.Agent.SetDestination(enemy.player.position);
            yield return null;
        }
        enemy.Agent.ResetPath();
        enemy.Agent.velocity = Vector3.zero;

        enemy.Anim.SetBool("isMoving", false);
        enemy.Anim.SetTrigger("Pattern2");
    }

    #endregion

    #region 패턴 3 - 지진
    IEnumerator Pattern3(EnemyBase enemy)
    {
        isAttacking = true;
        enemy.Anim.SetBool("isMoving", true);
        while (Vector3.Distance(enemy.player.position, enemy.transform.position) >= 8f)
        {
            RotateToPlayer(enemy, 5f);
            enemy.Agent.SetDestination(enemy.player.position);
            yield return null;
        }
        enemy.Agent.ResetPath();
        enemy.Agent.velocity = Vector3.zero;
        enemy.Anim.SetBool("isMoving", false);
        pattern3Warning.SetActive(true);
        yield return new WaitForSeconds(2f);
        pattern3Warning.SetActive(false);
        enemy.Anim.SetTrigger("Pattern3");
    }

    #endregion

    #region 패턴 4 - 돌진
    IEnumerator Pattern4(EnemyBase enemy)
    {
        isAttacking = true;
        if (enemy is Stage2Boss boss) boss.canRush = false;
        enemy.Anim.SetTrigger("Pattern4");
        pattern4Effect.SetActive(true);

        Vector3 dir = enemy.player.position - enemy.transform.position;
        dir.y = 0;
        dir.Normalize();
        enemy.transform.rotation = Quaternion.LookRotation(dir);

        float timer = 0f;
        float dashTime = 3f;
        while (timer < dashTime)
        {
            enemy.transform.position += dir * 20f * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
        pattern4Effect.SetActive(false);

        isAttacking = false;
        enemy.StartCoroutine(enemy.AttackDelay(5f));
    }

    #endregion

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
