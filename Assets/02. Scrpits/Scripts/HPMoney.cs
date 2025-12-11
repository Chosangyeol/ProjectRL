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


        [SerializeField]
        private string _interactName;
        public string interactName => _interactName;

        public void OnFocus()
        {
            interactCanvas.SetActive(true);
            interactCanvas.GetComponentInChildren<TMP_Text>().text = interactName + " - " + price + "G";
            Transform player = GameObject.FindAnyObjectByType<PlayerModel>().transform;

            // 플레이어 방향 벡터 계산
            Vector3 dir = player.position - interactCanvas.transform.position;
            dir.y = 0;  // 위아래 각도 제거

            // 방향이 0 벡터가 되지 않도록 체크
            if (dir.sqrMagnitude > 0.0001f)
            {
                interactCanvas.transform.rotation = Quaternion.LookRotation(dir);
            }
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