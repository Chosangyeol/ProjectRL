using Config;
using Player;
using Player.Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace UI.InventoryUI
{
    public class InventoryUI : MonoBehaviour
    {
        ConfigUserInput CUI;
        Inventory Inv;

        public GameObject InventoryPanel;
        public GameObject SmallInvPanel;
        public GameObject BigInvPanel;
        public GameObject[] SmallInvItemIcon;
        public GameObject[] BigInvItemIcon;

        public GameObject ItemDescPanel;
        public GameObject ItemDescPanelIcon;
        public GameObject ItemDescPanelName;
        public GameObject ItemDescPanelDesc;

        public GameObject ItemGetAnnouncePanel;
        public GameObject ItemGetAnnouncePanelIcon;
        public GameObject ItemGetAnnouncePanelName;
        public GameObject ItemGetAnnouncePanelDesc;

        bool OpenBigPanel;

        private void Start()
        {
            for(int i = 0; i < SmallInvItemIcon.Length; i++)
            {
                SmallInvItemIcon[i] = SmallInvPanel.transform.GetChild(i).gameObject;
                SmallInvItemIcon[i].SetActive(false);
            }
            for (int i = 0; i < BigInvItemIcon.Length; i++)
            {
                BigInvItemIcon[i] = BigInvPanel.transform.GetChild(i).gameObject;
                BigInvItemIcon[i].SetActive(false);
            }
            Inv = FindFirstObjectByType<PlayerModel>().Inventory;

            Inv.ActionAfterAddItem += OnItemAdded;
            Inv.ActionAfterRemoveItem += OnItemRemoved;

            CloseBigInv();
            OpenBigPanel = false;
        }

        void Update()
        {
            if (ConfigUserInput.Instance.GetKeyDown("keyInventory"))
            {
                if (OpenBigPanel)
                {
                    InventoryPanel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 710f), 1f).SetEase(Ease.OutCubic);
                    CloseBigInv();
                    OpenBigPanel = false;
                }
                else
                {
                    InventoryPanel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 210f), 1f).SetEase(Ease.OutCubic);
                    OpenBigInv();
                    OpenBigPanel = true;
                }
            }
        }

        public void ItemGetAnnounce(AItem item)
        {
            ItemGetAnnouncePanelIcon.GetComponent<Image>().sprite = item.itemData.itemSprite;
            ItemGetAnnouncePanelName.GetComponent<TMP_Text>().text = item.itemData.itemName;
            ItemGetAnnouncePanelDesc.GetComponent<TMP_Text>().text = item.itemData.tooltip;
            ItemGetAnnouncePanel.SetActive(true);
            StartCoroutine(Wait(3));      
        }
        void SmallInventoryIconUpdate()
        {
            for (int i = 0; i < Inv.Items.Count && i < 9; i++){
                SmallInvItemIcon[i].GetComponent<Image>().sprite = Inv.Items[i].itemData.itemSprite;
                SmallInvItemIcon[i].SetActive(true);
            }
        }
        void BigInventoryIconUpdate()
        {
            for (int i = 0; i < Inv.Items.Count; i++)
            {
                BigInvItemIcon[i].GetComponent<Image>().sprite = Inv.Items[i].itemData.itemSprite;
                BigInvItemIcon[i].SetActive(true);
            }
        }

        public void MouseOverIcon(int index)
        {
            ItemDescPanelIcon.GetComponent<Image>().sprite = Inv.Items[index].itemData.itemSprite;
            ItemDescPanelName.GetComponent<TMP_Text>().text = Inv.Items[index].itemData.itemName;
            ItemDescPanelDesc.GetComponent<TMP_Text>().text = Inv.Items[index].itemData.tooltip;
        }

        void OpenBigInv()
        {
            BigInvPanel.SetActive(true);
            SmallInvPanel.SetActive(false);
        }
        void CloseBigInv()
        {
            BigInvPanel.SetActive(false);
            SmallInvPanel.SetActive(true);
        }

        private void OnItemAdded(AItem item)
        {
            SmallInventoryIconUpdate();
            BigInventoryIconUpdate();
            ItemGetAnnounce(item);
        }

        private void OnItemRemoved(AItem item)
        {
            SmallInventoryIconUpdate();
            BigInventoryIconUpdate();
        }


        IEnumerator Wait(int second)
        {
            yield return new WaitForSeconds(second);
            ItemGetAnnouncePanel.SetActive(false);
            ItemGetAnnouncePanel.GetComponent<DOTweenAnimation>().DORewind();
        }
    }
}
