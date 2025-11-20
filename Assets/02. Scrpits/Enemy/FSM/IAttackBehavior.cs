using Info;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackBehavior
{
    void ExecuteAttack(EnemyBase enemy, int pattenrIndex = 0);
}

public class MeleeAttack : IAttackBehavior
{
    public void ExecuteAttack(EnemyBase enemy, int pattenrIndex = 0)
    {
        enemy.Anim.SetTrigger("Attack");
        enemy.StartAttackCoroutine(enemy.AttackDelay(2f));
    }
}

public class RangedAttack : IAttackBehavior
{
    public GameObject projectile;
    public Transform firePos;
    public float preAttackTime;

    public RangedAttack(GameObject projectilePrefab, Transform firePos, float preAttackTime)
    {
        this.projectile = projectilePrefab;
        this.firePos = firePos;
        this.preAttackTime = preAttackTime;
    }
    public void ExecuteAttack(EnemyBase enemy, int pattenrIndex = 0)
    {
        enemy.StartAttackCoroutine(AttackRoutine(enemy));
    }

    IEnumerator AttackRoutine(EnemyBase enemy)
    {
        float timer = 0f;

        enemy.Anim.SetTrigger("RangePreAttack");

        enemy.Lr.enabled = true;
        while (timer < preAttackTime)
        {
            FacePlayer(enemy);
            timer += Time.deltaTime;
            yield return null;
        }
        enemy.Lr.enabled = false;
        Shot(enemy);
        enemy.StartAttackCoroutine(enemy.AttackDelay(2f));
    }

    private void FacePlayer(EnemyBase enemy)
    {
        Vector3 dir = (enemy.player.transform.position - enemy.transform.position).normalized;
        dir.y = 0; // 고개만 돌고 위아래 각도는 무시
        enemy.transform.forward = dir;
       
        enemy.Lr.SetPosition(0, firePos.position);
        enemy.Lr.SetPosition(1, enemy.player.GetComponent<Collider>().bounds.center);
    }

    private void Shot(EnemyBase enemy)
    {
        enemy.Anim.SetTrigger("RangeAttack");
        PoolableMono proj = PoolManager.Instance.Pop(projectile.gameObject.name);
        proj.GetComponent<Projectile>().owner = enemy;
        proj.GetComponent<Projectile>().damage = enemy.GetStat().totalDamage;
        proj.transform.position = firePos.position;
        Vector3 targetPos = enemy.player.GetComponent<Collider>().bounds.center;
        proj.GetComponent<Rigidbody>().velocity =
            (targetPos - firePos.position).normalized * proj.GetComponent<Projectile>().speed;
    }
}

public class Stage1BossAttack : IAttackBehavior
{
    public GameObject pattern2Projectile;
    public Transform firePos;
    public Stage1BossAttack(GameObject pattern2Projectile, Transform firePos)
    {
        this.pattern2Projectile = pattern2Projectile;
        this.firePos = firePos;
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
                Pattenr2(enemy);
                break;
            case 2:
                Debug.Log(patternIndex + 1 + "번 패턴");
                Pattern3(enemy);
                break;
        }
        enemy.StartAttackCoroutine(enemy.AttackDelay(5f));

    }

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
    }

    IEnumerator Pattern1(EnemyBase enemy)
    {
        // 전방 내려찍기
        Debug.Log("패턴1 실행");
        enemy.Lr.startWidth = 0.1f;
        enemy.Lr.endWidth = 0.1f;
        float width = 7f;     // 박스 폭
        float length = 20f;

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
        enemy.Lr.enabled = false;
        yield return new WaitForSeconds(1f);
        DamageBoxActive(enemy, width, length, 2f);
    }

    private void Pattenr2(EnemyBase enemy)
    {

    }

    private void Pattern3(EnemyBase enemy)
    {

    }


}