using Info;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3BossAttack : IAttackBehavior
{
    public GameObject centor;
    public GameObject pattern2Projectile;
    public GameObject pattern2Warning;
    public Transform pattenr3FirePos;
    public GameObject pattern3Projectile;
    private bool hasPattern1Hit = false;
    private float timer = 0f;

    public bool isAttacking = false;

    public Stage3BossAttack(GameObject centor, GameObject pattern2Projectile, GameObject pattern2Warning, Transform pattenr3FirePos, GameObject pattern3Projectile)
    {
        this.centor = centor;
        this.pattern2Projectile = pattern2Projectile;
        this.pattern2Warning = pattern2Warning;
        this.pattenr3FirePos = pattenr3FirePos;
        this.pattern3Projectile = pattern3Projectile;
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


    IEnumerator Pattern1(EnemyBase enemy)
    {
        isAttacking = true;
        hasPattern1Hit = false;
        timer = 0f;

        // 패턴 1 구현
        Vector3 targetPos = enemy.player.GetComponent<Collider>().bounds.center;
        Vector3 dir = (targetPos - enemy.transform.position).normalized;

        enemy.Lr.SetPosition(0, enemy.transform.position);
        enemy.Lr.SetPosition(1, targetPos);
        enemy.Lr.enabled = true;

        while (timer < 1f)
        {
            timer += Time.deltaTime;
            FacePlayer(enemy);
            yield return null;
        }

        enemy.Lr.enabled = false;
        enemy.Anim.SetTrigger("Pattern1");

        while (true)
        {
            enemy.transform.position += dir * 20f * Time.deltaTime;
            if (!hasPattern1Hit)
            {
                Collider[] cols = Physics.OverlapSphere(enemy.transform.position, 3f);
                foreach (var col in cols)
                {
                    PlayerModel player = col.GetComponentInChildren<PlayerModel>();
                    if (player != null)
                    {
                        hasPattern1Hit = true;

                        SInfoAttack damage = new SInfoAttack(
                            enemy.gameObject,
                            player.gameObject,
                            Mathf.RoundToInt(enemy.GetStat().totalDamage),
                            null
                        );
                        player.Damaged(damage);
                        hasPattern1Hit = true;
                        yield return RecoverHeight(enemy);
                        yield break;
                    }
                }
            }

            float dist = Vector3.Distance(enemy.transform.position, targetPos);

            if (dist <= 1f)
            {
                yield return RecoverHeight(enemy);
                yield break;
            }
            yield return null;
        }
    }

    private void FacePlayer(EnemyBase enemy)
    {
        Vector3 dir = (enemy.player.transform.position - enemy.transform.position).normalized;
        dir.y = 0; // 고개만 돌고 위아래 각도는 무시
        enemy.transform.forward = dir;

        enemy.Lr.SetPosition(0, enemy.GetComponent<Collider>().bounds.center);
        enemy.Lr.SetPosition(1, enemy.player.GetComponent<Collider>().bounds.center);
    }

    IEnumerator RecoverHeight(EnemyBase enemy)
    {
        float speed = 20f;
        Vector3 dir = (centor.transform.position - enemy.transform.position).normalized;


        while (true)
        {
            enemy.transform.position += dir * speed * Time.deltaTime;
            yield return null;
            if (Vector3.Distance(centor.transform.position, enemy.transform.position) < 0.1f)
            {
                enemy.StartAttackCoroutine(enemy.AttackDelay(4f));
                isAttacking = false;
                yield break;
            }
        }
    }

    IEnumerator Pattern2(EnemyBase enemy)
    {
        isAttacking = true;

        float rangeX = 50f;
        float rangeZ = 50f;

        int count = 0;
        Vector3 targetPos = enemy.transform.position + Vector3.up * 10f;

        while (Vector3.Distance(enemy.transform.position, targetPos) > 0.1f)
        {
            enemy.transform.position += Vector3.up * 5f * Time.deltaTime;
            yield return null;
        }

        while (count < 100)
        {
            PoolableMono obj = PoolManager.Instance.Pop(pattern2Projectile.gameObject.name);
            PoolableMono warning = PoolManager.Instance.Pop(pattern2Warning.gameObject.name);
            Projectile proj = obj.GetComponent<Projectile>();

            proj.owner = enemy;
            proj.damage = enemy.GetStat().totalDamage;

            proj.transform.position = new Vector3(
                Random.Range(centor.transform.position.x - rangeX, centor.transform.position.x + rangeX),
                40f,
                Random.Range(centor.transform.position.z - rangeZ, centor.transform.position.z + rangeZ)
            );

            RaycastHit hit;
            if (Physics.Raycast(obj.transform.position, Vector3.down, out hit, 100f, LayerMask.GetMask("Ground")))
            {
                warning.transform.position = hit.point + Vector3.up * 0.1f;
                enemy.StartAttackCoroutine(DestroyWarning(warning));
            }
            proj.GetComponent<Rigidbody>().velocity = Vector3.down * proj.GetComponent<Projectile>().speed;
            count++;
            yield return new WaitForSeconds(0.05f);
        }

        while (Vector3.Distance(enemy.transform.position, centor.transform.position) > 0.1f)
        {
            enemy.transform.position += Vector3.down * 5f * Time.deltaTime;
            yield return null;
        }
        isAttacking = false;
        enemy.StartAttackCoroutine(enemy.AttackDelay(4f));
    }

    IEnumerator DestroyWarning(PoolableMono obj)
    {
        yield return new WaitForSeconds(1.3f);
        PoolManager.Instance.Push(obj);
    }

    IEnumerator Pattern3(EnemyBase enemy)
    {
        isAttacking = true;

        for (int i = 0; i < 10; i++)
        {
            PoolableMono obj = PoolManager.Instance.Pop(pattern3Projectile.gameObject.name);
            obj.transform.position = pattenr3FirePos.position;
            FacePlayer(enemy);
            obj.transform.forward = enemy.player.transform.position - pattenr3FirePos.position;
            Projectile proj = obj.GetComponent<Projectile>();
            Vector3 dir =
                (enemy.player.position - pattenr3FirePos.position).normalized;
            proj.owner = enemy;
            proj.damage = enemy.GetStat().totalDamage;

            obj.GetComponent<Rigidbody>().velocity = dir.normalized * proj.speed;
            yield return new WaitForSeconds(0.5f);

        }
        isAttacking = false;

        enemy.StartAttackCoroutine(enemy.AttackDelay(4f));

    }
}
