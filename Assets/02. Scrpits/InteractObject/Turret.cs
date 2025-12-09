using Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Turret
{
    public class Turret : InteractableObject, IInteractable
    {
        public GameObject bulletPrefab;
        public Transform firePoint;
        public float fireRate = 1f;
        public float detectRange = 10f;

        public GameObject objectToDisable;

        private float FireTime = 0f;
        private PlayerModel plyr;
        private bool isActivated = false;

        void Update()
        {
            FireTime += Time.deltaTime;

            if (Vector3.Distance(transform.position, plyr.transform.position) <= interactRange && !isActivated)
            {
                OnFocus();
            }
            else
            {
                OnUnFocus();
            }

            if (isActivated)
            {
                Collider[] cols = Physics.OverlapSphere(this.transform.position, detectRange, LayerMask.GetMask("Enemy"));
                if (cols != null)
                {
                    if (FireTime >= fireRate)
                    {
                        Shoot();
                        FireTime = 0;
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
            // 범위 안에 적 탐지 후 적 사격

            //Vector3 dir = (player.position - firePoint.position).normalized;
            //Quaternion rot = Quaternion.LookRotation(dir);
        }

        public string interactName { get; }

        public void OnFocus()
        {
            interactCanvas.SetActive(true);
            interactCanvas.GetComponentInChildren<TMP_Text>().text = interactName + " - " + price + "G";
        }
        public void OnUnFocus()
        {
            interactCanvas.SetActive(false);
            interactCanvas.GetComponentInChildren<TMP_Text>().text = " ";
        }
        public void OnInteract()
        {
            int nowMoney = GameManager.Instance.Money;

            if (nowMoney >= price)
            {
                GameManager.Instance.RemoveMoney(price);
                isActivated = true;                 // 한 번만 열림
            }
            else
            {
                Debug.Log("돈 부족");
            }
        }
    }
}
