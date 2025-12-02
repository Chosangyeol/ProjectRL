using Info;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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

    public bool isSpawnObj = false;
    public PoolableMono spawnObj;

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
        if (other.CompareTag("Player"))
        {
            SInfoAttack attackInfo = new SInfoAttack(
                owner.gameObject,
                other.gameObject,
                Mathf.RoundToInt(damage),
                null
            );

            other.GetComponentInChildren<PlayerModel>().Damaged(attackInfo);
            Debug.Log($"Player Damaged : {damage}");

            if (isSpawnObj)
            {
                // ÀåÆÇ »ý¼º
                SpawnObjcet(other.transform.position);
            }
            PoolManager.Instance.Push(this);
        }

        if (other.CompareTag("Ground"))
        {
            if (isSpawnObj)
            {
                SpawnObjcet(transform.position);
            }
        }
    }

    public void SpawnObjcet(Vector3 origin)
    {
        if (!isSpawnObj) return;

        RaycastHit hit;
        Vector3 rayStart = origin + Vector3.up * 2f;

        if (Physics.Raycast(rayStart, Vector3.down, out hit, 10f, LayerMask.GetMask("Ground")))
        {
            PoolableMono spawnObj = PoolManager.Instance.Pop(this.spawnObj.name);

            Vector3 pos = hit.point;
            pos.y += 0.05f; // »ìÂ¦ ¶ç¿ö¼­ Áö¸é °£¼· Á¦°Å
            spawnObj.transform.position = pos;
        }
    }
}
