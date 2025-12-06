using Info;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Melee2 : EnemyBase
{
    protected override void Start()
    {
        base.Start();

        attackBehavior = new MeleeAttack();
    }

    public void Attack()
    {
        Vector3 size = new Vector3(1.25f, 3f, 1.25f);
        Vector3 centor = transform.position + transform.forward * 1.25f;
        Collider[] hit = Physics.OverlapBox(centor, size);

        foreach (Collider col in hit)
        {
            PlayerModel other = col.GetComponentInChildren<PlayerModel>();

            if (other != null)
            {
                SInfoAttack attack = new SInfoAttack(
                    this.gameObject,
                    other.gameObject,
                    Mathf.RoundToInt(Stat.totalDamage),
                    null
                    );

                audioS.clip = attackClip;
                audioS.Play();
                other.Damaged(attack);
            }
        }
    }

    public void StartAttackDelay()
    {
        StartCoroutine(AttackDelay(enemySO.attackSpeed));
    }
}
