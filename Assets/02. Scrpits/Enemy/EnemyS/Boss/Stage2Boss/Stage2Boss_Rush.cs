using Info;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Stage2Boss_Rush : MonoBehaviour
{
    public bool isHit = false;
    public EnemyBase owner;
    public float damage;

    public void Init(EnemyBase owner, float damage)
    {
        this.owner = owner;
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isHit)
        {
            PlayerModel model = other.GetComponentInChildren<PlayerModel>();

            SInfoAttack attackInfo = new SInfoAttack(
                owner.gameObject,
                other.gameObject,
                Mathf.RoundToInt(damage),
                null
            );

            model.Damaged(attackInfo);
            isHit = true;
        }
    }

}
