using Info;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Boss_Rock : PoolableMono
{
    private EnemyBase owner;
    private Transform targetPos;
    private Rigidbody rb;
    public float delay = 5f;
    public float rockSpeed = 5f;
    public float spinSpeed = 5f;
    public int damage = 40;


    public void InitRock(Transform pos, float delay, EnemyBase owner)
    {
        targetPos = pos;
        this.owner = owner;
        this.delay = delay;
        rb = GetComponent<Rigidbody>();
    }

    public void Shot()
    {
        StartCoroutine(FireRock());
        StartCoroutine(SpinRock());
    }

    IEnumerator FireRock()
    {
        yield return new WaitForSeconds(delay);

        Vector3 dir = (targetPos.position - transform.position).normalized;
        while (true)
        {
            transform.position += dir * rockSpeed * Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator SpinRock()
    {
        while (true)
        {
            Vector3 dir = (targetPos.position - transform.position).normalized;
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = lookRot * Quaternion.Euler(spinSpeed * Time.time, 0, 0);
            yield return null;
        }  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerModel player = other.GetComponentInChildren<PlayerModel>();

            SInfoAttack attackInfo = new SInfoAttack(
                owner.gameObject,
                other.gameObject,
                damage,
                null
                );

            player.Damaged(attackInfo);
        }
    }
}
