using Info;
using Player;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoisonEffect : MonoBehaviour
{
    public int damage;
    public bool isActive = false;
    public bool alreadyDamaged = false;
    public float timer = 0f;

    private void Update()
    {
        if (isActive)
        {
            timer += Time.deltaTime;
            if (timer >= 1f)
            {
                alreadyDamaged = false;
                timer = 0f;
            }
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !alreadyDamaged)
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
                alreadyDamaged = true;
            }
        }
    }
}
