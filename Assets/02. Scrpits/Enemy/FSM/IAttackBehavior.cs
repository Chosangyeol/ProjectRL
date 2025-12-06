using Info;
using Player;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public interface IAttackBehavior
{
    void ExecuteAttack(EnemyBase enemy, int pattenrIndex = 0);
}

public class MeleeAttack : IAttackBehavior
{
    public void ExecuteAttack(EnemyBase enemy, int pattenrIndex = 0)
    {
        enemy.Anim.SetTrigger("Attack");
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
        enemy.StartAttackCoroutine(enemy.AttackDelay(enemy.GetStat().attackSpeed));
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
        enemy.Anim.SetTrigger("Attack");

        if (enemy.attackClip != null)
        {
            enemy.AudioS.clip = enemy.attackClip;
            enemy.AudioS.Play();
        }

        PoolableMono proj = PoolManager.Instance.Pop(projectile.gameObject.name);
        proj.GetComponent<Projectile>().owner = enemy;
        proj.GetComponent<Projectile>().damage = enemy.GetStat().totalDamage;
        proj.transform.position = firePos.position;
        Vector3 targetPos = enemy.player.GetComponent<Collider>().bounds.center;
        proj.GetComponent<Rigidbody>().velocity =
            (targetPos - firePos.position).normalized * proj.GetComponent<Projectile>().speed;
    }
}

public class Stage1EliteAttack : IAttackBehavior
{
    public bool isAttacking = false;

    private PoolableMono projectile;
    private Transform firePos1;
    private Transform firePos2;

    public Stage1EliteAttack(PoolableMono projectile, Transform firePos1, Transform firePos2)
    {
        this.projectile = projectile;
        this.firePos1 = firePos1; 
        this.firePos2 = firePos2;
    }

    public void ExecuteAttack(EnemyBase enemy, int patternIndex = 0)
    {
        if (enemy is Stage1Elite elite)
        {
            if (Vector3.Distance(enemy.player.position, enemy.transform.position) >= 15f && elite.canRange)
            {
                enemy.StartCoroutine(AttackRoutine(enemy));
                elite.canRange = false;
                return;
            }

            enemy.StartCoroutine(MeleeAttack(enemy));
        }
    }

    IEnumerator MeleeAttack(EnemyBase enemy)
    {
        isAttacking = true;

        enemy.Anim.SetBool("Chase", true);
        while (Vector3.Distance(enemy.player.position, enemy.transform.position) >= 4f)
        {
            RotateToPlayer(enemy, 5f);
            enemy.Agent.SetDestination(enemy.player.position);
            yield return null;
        }
        enemy.Agent.ResetPath();
        enemy.Agent.velocity = Vector3.zero;

        enemy.Anim.SetBool("Chase", false);
        enemy.Anim.SetTrigger("MeleeAttack");
        enemy.StartAttackCoroutine(enemy.AttackDelay(6f));

    }

    IEnumerator AttackRoutine(EnemyBase enemy)
    {
        float timer = 0;

        enemy.Anim.SetTrigger("RangePreAttack");
        enemy.Lr.positionCount = 3;

        enemy.Lr.enabled = true;

        while (timer < 3f)
        {
            FacePlayer(enemy);
            timer += Time.deltaTime;
            yield return null;
        }
        enemy.Lr.enabled = false;
        Shot(enemy);
        enemy.StartAttackCoroutine(enemy.AttackDelay(2f));


    }

    public void Shot(EnemyBase enemy)
    {
        enemy.Anim.SetTrigger("RangeAttack");
        PoolableMono proj = PoolManager.Instance.Pop(projectile.gameObject.name);
        proj.GetComponent<Projectile>().owner = enemy;
        proj.GetComponent<Projectile>().damage = (enemy.GetStat().totalDamage) / 2;
        proj.transform.position = firePos1.position;
        Vector3 targetPos = enemy.player.GetComponent<Collider>().bounds.center;
        proj.GetComponent<Rigidbody>().velocity =
            (targetPos - firePos1.position).normalized * proj.GetComponent<Projectile>().speed;

        PoolableMono proj2 = PoolManager.Instance.Pop(projectile.gameObject.name);
        proj2.GetComponent<Projectile>().owner = enemy;
        proj2.GetComponent<Projectile>().damage = (enemy.GetStat().totalDamage) / 2;
        proj2.transform.position = firePos2.position;
        Vector3 targetPos2 = enemy.player.GetComponent<Collider>().bounds.center;
        proj2.GetComponent<Rigidbody>().velocity =
            (targetPos2 - firePos2.position).normalized * proj2.GetComponent<Projectile>().speed;
    }

    private void RotateToPlayer(EnemyBase enemy, float rotSpeed)
    {
        Vector3 dir = (enemy.player.position - enemy.transform.position);
        dir.y = 0;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRot, Time.deltaTime * rotSpeed);
    }

    private void FacePlayer(EnemyBase enemy)
    {
        Vector3 dir = (enemy.player.transform.position - enemy.transform.position).normalized;
        dir.y = 0; // 고개만 돌고 위아래 각도는 무시
        enemy.transform.forward = dir;

        enemy.Lr.SetPosition(0, firePos1.position);
        enemy.Lr.SetPosition(1, enemy.player.GetComponent<Collider>().bounds.center);
        enemy.Lr.SetPosition(2, firePos2.position);
    }
}

public class Stage2Melee1Attack : IAttackBehavior
{
    public float rushSpeed = 15f;

    public Stage2Melee1Attack(float rushSpeed)
    {
        this.rushSpeed = rushSpeed;
    }
    public void ExecuteAttack(EnemyBase enemy, int pattenrIndex = 0)
    {
        enemy.StartCoroutine(Rush(enemy));
    }

    IEnumerator Rush(EnemyBase enemy)
    {
        enemy.Anim.SetBool("Chase", false);
        enemy.Anim.SetTrigger("Idle");
        float t = 0;
        float timer = 2;
        enemy.Lr.enabled = true;
        while (t < timer)
        {
            t += Time.deltaTime;
            FacePlayer(enemy);
            yield return null;
        }
        enemy.Lr.enabled = false;

        Vector3 dir = (enemy.player.position - enemy.transform.position);
        dir.y = 0;
        dir.Normalize();

        t = 0;
        enemy.Anim.SetBool("Rush", true);
        while (t < timer)
        {
            t += Time.deltaTime;
            enemy.transform.position += dir * rushSpeed * Time.deltaTime;
            yield return null;
        }
        enemy.Anim.SetBool("Rush", false);
        enemy.StartAttackCoroutine(enemy.AttackDelay(enemy.GetStat().attackSpeed));
    }

    private void FacePlayer(EnemyBase enemy)
    {
        Vector3 dir = (enemy.player.transform.position - enemy.transform.position).normalized;
        dir.y = 0; // 고개만 돌고 위아래 각도는 무시
        enemy.transform.forward = dir;

        enemy.Lr.SetPosition(0, enemy.GetComponent<Collider>().bounds.center);
        enemy.Lr.SetPosition(1, enemy.player.GetComponent<Collider>().bounds.center);
    }
}

public class Stage3MeleeAttack : IAttackBehavior
{
    public void ExecuteAttack(EnemyBase enemy, int pattenrIndex = 0)
    {
        
        enemy.StartCoroutine(JumpParabolaAttack(enemy,enemy.player));
    }
    IEnumerator JumpParabolaAttack(EnemyBase enemy, Transform player)
    {
        enemy.Anim.SetBool("Chase", false);
        enemy.Anim.SetTrigger("Idle");
        yield return new WaitForSeconds(2f);

        enemy.Agent.enabled = false; // NavMeshAgent 중지

        Vector3 startPos = enemy.transform.position;
        Vector3 targetPos = player.position;

        float duration = 1.0f;      // 점프 시간
        float height = 4.0f;        // 포물선 최고 높이
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            // 직선 이동
            Vector3 pos = Vector3.Lerp(startPos, targetPos, t);

            // 포물선 곡선 추가
            float yOffset = height * Mathf.Sin(Mathf.PI * t);

            pos.y += yOffset;

            enemy.transform.position = pos;

            yield return null;
        }
        enemy.Anim.SetTrigger("Attack");
        // 착지
        enemy.Agent.enabled = true;

        enemy.StartAttackCoroutine(enemy.AttackDelay(enemy.GetStat().attackSpeed));

    }

}



