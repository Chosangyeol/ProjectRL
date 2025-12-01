using Info;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : PoolableMono
{
    public EnemyBase owner;
    [HideInInspector]
    public float damage;
    [HideInInspector]
    public float timer;
    public float destroyTime;
    public float speed;

    public bool isEffect = false;

    public override void Reset()
    {
        base.Reset();
        timer = Time.time;
    }

    private void Update()
    {
        if (Time.time - timer >= destroyTime)
        {
            PoolManager.Instance.Push(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isEffect)
        {
            SInfoAttack attackInfo = new SInfoAttack(
                owner.gameObject,
                other.gameObject,
                Mathf.RoundToInt(damage),
                null
            );
            other.GetComponentInChildren<PlayerModel>().Damaged(attackInfo);
            Debug.Log($"Player Damaged : {damage}");
            PoolManager.Instance.Push(this);
        }
    }
}
