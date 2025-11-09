using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Turret
{
    public class Turret : MonoBehaviour
    {
        public GameObject bulletPrefab;
        public Transform firePoint;
        public float fireRate = 1f;
        public float detectRange = 10f;

        public GameObject objectToDisable;

        private float FireTime = 0f;
        private Transform player;
        private bool isActivated = false;

        void Update()
        {
            if (player == null)
            {
                GameObject p = GameObject.FindGameObjectWithTag("Player");
                if (p != null) player = p.transform;
            }

            if (player != null)
            {
                float dist = Vector3.Distance(transform.position, player.position);

                if (!isActivated && dist <= detectRange)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        Player playerScript = player.GetComponent<Player>();
                        if (playerScript != null && playerScript.money >= 25)
                        {
                            playerScript.money -= 25;
                            isActivated = true;
                            Debug.Log("터렛 활성");


                            if (objectToDisable != null)
                            {
                                Destroy(objectToDisable);
                            }
                        }
                        else
                        {
                            Debug.Log("돈 부족.");
                        }
                    }
                }

                if (isActivated && dist <= detectRange)
                {
                    Vector3 dir = (player.position - transform.position).normalized;
                    dir.y = 0f;
                    transform.rotation = Quaternion.LookRotation(dir);

                    if (Time.time >= FireTime)
                    {
                        Shoot();
                        FireTime = Time.time + 1f / fireRate;
                    }
                }
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
            Gizmos.DrawWireSphere(transform.position, detectRange);
        }

        void Shoot()
        {
            if (bulletPrefab != null && firePoint != null && player != null)
            {
                Vector3 dir = (player.position - firePoint.position).normalized;
                Quaternion rot = Quaternion.LookRotation(dir);
                Instantiate(bulletPrefab, firePoint.position, rot);
            }
        }
    }
}
