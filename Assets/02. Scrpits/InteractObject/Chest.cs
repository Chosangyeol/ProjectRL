using Config;
using Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace chest
{
    public class Chest : InteractableObject, IInteractable
    {
        public DropTableSO items; // 아이템 프리팹들

        private PlayerModel plyr;
        private bool opened = false;   // 1회용 상자를 위한 변수

        protected override void Start()
        {
            base.Start();
            plyr = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerModel>();
        }

        void Update()
        {
            if (Vector3.Distance(transform.position, plyr.transform.position) <= interactRange && !opened)
            {
                OnFocus();
            }
            else
            {
                OnUnFocus();
            }
        }

        public void TryDropItem(DropTableSO table)
        {
            if (table == null) return;

            // 드랍될 아이템의 등급 정하기
            float groupResult = Random.Range(0f, 100f);
            float groupWeight = 0f;

            DropTableSO.RarityGroup selectedGroup = null;

            foreach (var group in table.rarityGroup)
            {
                groupWeight += group.rarityWeight;
                if (groupResult <= groupWeight)
                {
                    selectedGroup = group;
                    break;
                }
            }

            if (selectedGroup == null || selectedGroup.items.Count == 0) return;

            // 정해진 등급 안에서 아이템 드랍하기
            float itemResult = Random.Range(0f, 100f);
            float itemWeight = 0f;

            PoolableMono selectedItem = null;

            foreach (var item in selectedGroup.items)
            {
                itemWeight += item.weight;
                if (itemResult <= itemWeight)
                {
                    selectedItem = item.item;
                    break;
                }
            }

            if (selectedItem != null)
            {
                PoolableMono dropItem = PoolManager.Instance.Pop(selectedItem.name);
                dropItem.gameObject.transform.position = this.gameObject.transform.position;
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
            interactCanvas.SetActive(false);
            interactCanvas.GetComponentInChildren<TMP_Text>().text = " ";
        }
        public void OnInteract()
        {
            if (opened) return;

            int nowMoney = GameManager.Instance.Money;

            if (nowMoney >= price)
            {
                GameManager.Instance.RemoveMoney(price);
                TryDropItem(items);                // 해당 태그 프리팹 소환
                opened = true;                 // 한 번만 열림
            }
            else
            {
                Debug.Log("돈 부족");
            }
        }
    }
}
