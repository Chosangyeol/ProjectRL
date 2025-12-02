using Info;
using Player;
using System.Collections;
using System.Collections.Generic;
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



