using Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace hpHeal
{
    public class HPHeal : InteractableObject, IInteractable
    {      
        private PlayerModel player;
        private bool isActivated = false;

        public GameObject targetObject; // 활성화할 오브젝트
        
        public float activeDuration = 4f; // 오브젝트 유지 시간(힐 장판)

        protected override void Start()
        {
            base.Start();
            player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerModel>();

            if (targetObject != null)
            {
                targetObject.SetActive(false);
            }
        }

        void Update()
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= interactRange && !isActivated)
            {
                OnFocus();
            }
            else
            {
                OnUnFocus();
            }
        }

        IEnumerator DeactivateAfterSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            if (targetObject != null)
            {
                targetObject.SetActive(false);
            }
            isActivated = false;
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

            if (nowMoney >= price)
            {
                GameManager.Instance.RemoveMoney(price);
                if (targetObject != null)
                {
                    targetObject.SetActive(true);
                    StartCoroutine(DeactivateAfterSeconds(activeDuration));
                }

                isActivated = true;                // 한 번만 열림
            }
            else
            {
                Debug.Log("돈 부족");
            }
        }
    }
}
