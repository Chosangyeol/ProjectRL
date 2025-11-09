using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dron
{
    public class Dron : MonoBehaviour
    {
        public GameObject bulletPrefab;
        public Transform firePoint;
        public float fireRate = 1f;
        public float detectRange = 10f;

        public float orbitRadius = 3f;
        public float orbitSpeed = 1f;
        public float moveSmoothness = 2f;

        public GameObject objectToDisable;

        private float FireTime = 0f;
        private Transform player;
        private bool isActivated = false;
        private float angle = 0f;

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
                        PlayerKM playerScript = player.GetComponent<PlayerKM>();
                        if (playerScript != null && playerScript.money >= 25)
                        {
                            playerScript.money -= 25;
                            isActivated = true;
                            Debug.Log("드론 활성");


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

                if (isActivated)
                {
                    FlyAroundPlayer();

                    if (dist <= detectRange)
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
        }

        void FlyAroundPlayer()
        {
            angle += orbitSpeed * Time.deltaTime;
            float x = Mathf.Cos(angle) * orbitRadius;
            float z = Mathf.Sin(angle) * orbitRadius;

            Vector3 targetPos = player.position + new Vector3(x, 2f, z);
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * moveSmoothness);
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
