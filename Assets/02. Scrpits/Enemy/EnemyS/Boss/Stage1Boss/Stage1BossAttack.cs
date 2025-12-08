using Info;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1BossAttack : IAttackBehavior
{
    public GameObject pattern2Projectile;
    public GameObject pattern3Projectile;
    public Transform firePos;
    public Transform missilePos;
    public GameObject pattern3Warning;

    public bool isAttacking = false;

    public Stage1BossAttack(GameObject pattern2Projectile, GameObject pattern3Projectile, Transform firePos, Transform missilePos, GameObject pattern3Warning)
    {
        this.pattern2Projectile = pattern2Projectile;
        this.pattern3Projectile = pattern3Projectile;
        this.firePos = firePos;
        this.missilePos = missilePos;
        this.pattern3Warning = pattern3Warning;
    }

    public void ExecuteAttack(EnemyBase enemy, int patternIndex = 0)
    {
        switch (patternIndex)
        {
            case 0:
                Debug.Log(patternIndex + 1 + "번 패턴");
                enemy.StartAttackCoroutine(Pattern1(enemy));
                break;
            case 1:
                Debug.Log(patternIndex + 1 + "번 패턴");
                enemy.StartAttackCoroutine(Pattern2(enemy));
                break;
            case 2:
                Debug.Log(patternIndex + 1 + "번 패턴");
                enemy.StartAttackCoroutine(Pattern3(enemy));

                break;
        }
    }

    #region 패턴 1 - 지면 강타
    private void DamageBoxActive(EnemyBase enemy, float width, float length, float offset)
    {
        Vector3 centor = enemy.transform.position + enemy.transform.forward * (offset + length / 2f);
        centor.y = 1f;

        Vector3 half = new Vector3(width / 2f, 1f, length / 2f);

        Quaternion rot = enemy.transform.rotation;

        Collider[] hits = Physics.OverlapBox(centor, half, rot);

        SInfoAttack damage = new SInfoAttack(
                    enemy.gameObject,
                    enemy.player.gameObject,
                    Mathf.RoundToInt(enemy.GetStat().totalDamage),
                    null
                );


        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                PlayerModel player = hit.GetComponentInChildren<PlayerModel>();
                if (player != null)
                {
                    player.Damaged(damage);
                }
            }
        }

        isAttacking = false;
    }

    IEnumerator Pattern1(EnemyBase enemy)
    {
        isAttacking = true;
        // 전방 내려찍기
        Debug.Log("패턴1 실행");
        enemy.Lr.startWidth = 0.1f;
        enemy.Lr.endWidth = 0.1f;
        float width = 12f;     // 박스 폭
        float length = 70f;

        Vector3 startPos = enemy.transform.position + enemy.transform.forward * 2;
        startPos.y = 0.1f;

        Vector3 p0 = startPos + enemy.transform.right * (width / 2) + enemy.transform.forward * 0;
        Vector3 p1 = startPos + enemy.transform.right * (-width / 2) + enemy.transform.forward * 0;
        Vector3 p2 = startPos + enemy.transform.right * (-width / 2) + enemy.transform.forward * length;
        Vector3 p3 = startPos + enemy.transform.right * (width / 2) + enemy.transform.forward * length;

        enemy.Lr.positionCount = 4;
        enemy.Lr.SetPosition(0, p0);
        enemy.Lr.SetPosition(1, p1);
        enemy.Lr.SetPosition(2, p2);
        enemy.Lr.SetPosition(3, p3);

        enemy.Lr.enabled = true;

        yield return new WaitForSeconds(2f);
        enemy.Anim.SetTrigger("Pattern1");
        enemy.Lr.enabled = false;
        yield return new WaitForSeconds(1f);
        DamageBoxActive(enemy, width, length, 2f);

        enemy.StartAttackCoroutine(enemy.AttackDelay(4f));
    }
    #endregion

    #region 패턴 2 - 샷건
    IEnumerator Pattern2(EnemyBase enemy)
    {
        isAttacking = true;
        int projectileCount = 5;     // 원하는 발사 수
        float angleStep = 10f;       // 양쪽으로 벌어지는 각도
        float startAngle = -(projectileCount - 1) / 2f * angleStep;
        enemy.Anim.SetTrigger("Pattern2");

        yield return new WaitForSeconds(0.5f);
        Vector3 forwardDir = enemy.transform.forward.normalized;
        float baseAngle = Mathf.Atan2(forwardDir.z, forwardDir.x) * Mathf.Rad2Deg;

        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < projectileCount; i++)
            {
                float angle = baseAngle + startAngle + angleStep * i;
                Vector3 shootDir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad));

                ShootProjectile(enemy, shootDir);
            }
            yield return new WaitForSeconds(2f);
        }
        isAttacking = false;
        enemy.StartAttackCoroutine(enemy.AttackDelay(3f));
    }

    private void ShootProjectile(EnemyBase enemy, Vector3 dir)
    {
        PoolableMono proj = PoolManager.Instance.Pop(pattern2Projectile.gameObject.name);

        Projectile p = proj.GetComponent<Projectile>();
        p.owner = enemy;
        p.damage = enemy.GetStat().totalDamage;

        proj.transform.position = firePos.position;
        proj.GetComponent<Rigidbody>().velocity = dir.normalized * p.speed;
    }
    #endregion

    #region 패턴 3 - 미사일
    IEnumerator Pattern3(EnemyBase enemy)
    {
        isAttacking = true;
        for (int i = 0; i < 5; i++)
        {
            if (enemy is Stage1Boss boss)
                boss.PlaySound(boss.Pattern3FireClip);
            PoolableMono obj = PoolManager.Instance.Pop(pattern3Projectile.gameObject.name);
            Projectile proj = obj.GetComponent<Projectile>();
            proj.transform.position = missilePos.position;
            Vector3 dir = missilePos.transform.up;
            proj.GetComponent<Rigidbody>().velocity = dir.normalized * proj.speed;
            yield return new WaitForSeconds(0.5f);
            PoolManager.Instance.Push(obj);
        }


        for (int i = 0; i < 5; i++)
        {
            Vector3 player = enemy.player.position + Vector3.up * 1f;
            RaycastHit hit;

            PoolableMono warning = PoolManager.Instance.Pop(pattern3Warning.gameObject.name);
            if (Physics.Raycast(player, Vector3.down, out hit, 100f, LayerMask.GetMask("Ground")))
            {
                warning.transform.position = hit.point;
            }
            yield return new WaitForSeconds(0.5f);
            enemy.StartAttackCoroutine(DestroyWarning(warning, enemy));

            PoolableMono obj = PoolManager.Instance.Pop(pattern3Projectile.gameObject.name);
            obj.transform.position = warning.transform.position + Vector3.up * 10f;
            Projectile proj = obj.GetComponent<Projectile>();
            Vector3 dir = Vector3.down;
            proj.GetComponent<Rigidbody>().velocity = dir.normalized * 10f;
            proj.owner = enemy;
            proj.damage = (enemy.GetStat().totalDamage)/2;
        }

        isAttacking = false;
        enemy.StartAttackCoroutine(enemy.AttackDelay(3f));
    }

    IEnumerator DestroyWarning(PoolableMono warning, EnemyBase enemy)
    {
        Debug.Log("경고 이펙트 시작");
        yield return new WaitForSeconds(1f);
        //폭발 이펙트
        Debug.Log("경고 이펙트 종료");

        if (enemy is Stage1Boss boss)
            boss.PlaySound(boss.Pattern3BoomClip);

        Vector3 centor = warning.transform.position + Vector3.up * 5f;

        Collider[] hits = Physics.OverlapCapsule(point0: centor + Vector3.up * 5f, point1: centor + Vector3.down * 5f, radius: 3f);

        SInfoAttack damage = new SInfoAttack(
                    enemy.gameObject,
                    enemy.player.gameObject,
                    Mathf.RoundToInt(enemy.GetStat().totalDamage),
                    null
                );


        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                PlayerModel player = hit.GetComponentInChildren<PlayerModel>();
                if (player != null)
                {
                    player.Damaged(damage);
                }
            }
        }
        PoolManager.Instance.Push(warning);
    }
    #endregion

}