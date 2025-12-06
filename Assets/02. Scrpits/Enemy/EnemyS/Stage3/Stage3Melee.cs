using Info;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3Melee : EnemyBase
{
    protected override void Start()
    {
        base.Start();

        attackBehavior = new Stage3MeleeAttack();
    }

    public void Attack()
    {

        Vector3 centor = transform.position + transform.up * 1.25f;
        Collider[] hit = Physics.OverlapSphere(centor, 2f);

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
}
