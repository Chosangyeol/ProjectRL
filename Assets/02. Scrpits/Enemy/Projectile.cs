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

    public override void Reset()
    {
        base.Reset();
        timer = Time.time;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (Time.time - timer >= destroyTime)
        {
            PoolManager.Instance.Push(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("�÷��̾� ����");
            PoolManager.Instance.Push(this);
        }
    }
}
