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
            Debug.Log("플레이어 적중");
            PoolManager.Instance.Push(this);
        }
    }
}
