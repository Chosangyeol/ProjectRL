using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace healBullet
{
    public class HealBullet : MonoBehaviour
    {
        public float speed = 5f;
        public int healAmount = 10;

        private Rigidbody rb;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = transform.forward * speed;
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerKM player = other.GetComponent<PlayerKM>();
                if (player != null)
                {
                    player.HP += healAmount;
                    Debug.Log("플레이어 회복 +" + healAmount + " → 현재 HP: " + player.HP);
                }

                Destroy(gameObject);
            }
            else if (!other.isTrigger)
            {
                Destroy(gameObject);
            }
        }
    }
}