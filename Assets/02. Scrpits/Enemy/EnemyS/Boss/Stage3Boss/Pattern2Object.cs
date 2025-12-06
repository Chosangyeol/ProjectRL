using Info;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern2Object : PoolableMono
{
    private Transform target;
    public int damage = 5;
    public float speed = 1f;

    public void SetTarget(Transform target)
    {
         this.target = target;
         
    }

    private void Update()
    {
        if (target == null) return;

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        transform.LookAt(target);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerModel player = other.GetComponentInChildren<PlayerModel>();
            if (player != null)
            {
                SInfoAttack attackInfo = new SInfoAttack(
                    this.gameObject,
                    player.gameObject,
                    damage
                    );

                player.Damaged(attackInfo);
                // ¿Ã∆Â∆Æ
                PoolManager.Instance.Push(this);
            }
        }

        if (other.GetComponent<PlayerBullet>())
        {
            target.GetComponentInChildren<PlayerModel>().Pool.Push(other.GetComponent<PoolableMono>());
            PoolManager.Instance.Push(this);
        }
    }
}
