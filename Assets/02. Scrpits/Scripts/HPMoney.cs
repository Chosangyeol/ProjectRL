using Info;
using Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace hpmoney
{
    public class HPMoney : InteractableObject, IInteractable
    {
        public float detectRange = 10f;
        private PlayerModel plyr;
        private bool isActivated = false;

        protected override void Start()
        {
            base.Start();
            plyr = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerModel>();
        }

        void Update()
        {
            if (Vector3.Distance(transform.position, plyr.transform.position) <= interactRange && !isActivated)
            {
                OnFocus();
            }
            else
            {
                OnUnFocus();
            }
        }


        public string interactName { get; }

        public void OnFocus()
        {
            interactCanvas.SetActive(true);
            interactCanvas.GetComponentInChildren<TMP_Text>().text = interactName + " - " + price + "G";
        }
        public void OnUnFocus()
        {
            interactCanvas.GetComponentInChildren<TMP_Text>().text = " ";
            interactCanvas.SetActive(false);
        }
        public void OnInteract()
        {
            if (isActivated) return;

            int nowMoney = GameManager.Instance.Money;
            PlayerModel player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerModel>();

            if (player.Stat.Stat.hpCurrent > price)
            {
                GameManager.Instance.AddMoney(price);

                SInfoAttack damage = new SInfoAttack(
                    this.gameObject,
                    player.gameObject,
                    price,
                    null
                    );

                player.Damaged(damage);
                isActivated = true;                 // 한 번만 열림
            }
            else
            {
                Debug.Log("체력 부족");
            }
        }
    }
}