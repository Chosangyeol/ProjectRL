using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Config;
using Player.Item;

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
            CloseBigInv();
            OpenBigPanel = false;
        }

        void Update()
        {
            if (CUI.GetKeyDown("keyInventory")) // I로 되어있긴 한데 TAB으로 바꾸자는 얘기 있었음
            {
                if (OpenBigPanel)
                {
                    CloseBigInv();
                    OpenBigPanel = false;
                    InventoryPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 710f);

                }
                else
                {
                    OpenBigInv();
                    OpenBigPanel = true;
                    InventoryPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 210f);
                }
            }
        }

        public void ItemGetAnnounce() //Inventory랑 연결하면 딱인디 어야 할까요 이거
        {
            SmallInventoryIconUpdate();
            BigInventoryIconUpdate();

            ItemGetAnnouncePanelIcon.GetComponent<Image>().sprite = Inv.items[Inv.items.Count].itemData.itemSprite;
            ItemGetAnnouncePanelName.GetComponent<Text>().text = Inv.items[Inv.items.Count].itemData.itemName;
            ItemGetAnnouncePanelDesc.GetComponent<Text>().text = Inv.items[Inv.items.Count].itemData.tooltip;
            ItemGetAnnouncePanel.SetActive(true);
            StartCoroutine(Wait(3));
            ItemGetAnnouncePanel.SetActive(false);
            
        }
        void SmallInventoryIconUpdate()
        {
            for (int i = 0; i < Inv.items.Count && i < 9; i++){
                SmallInvItemIcon[i].GetComponent<Image>().sprite = Inv.items[i].itemData.itemSprite;
                SmallInvItemIcon[i].SetActive(true);
            }
        }
        void BigInventoryIconUpdate()
        {
            for (int i = 0; i < Inv.items.Count; i++)
            {
                BigInvItemIcon[i].GetComponent<Image>().sprite = Inv.items[i].itemData.itemSprite;
                BigInvItemIcon[i].SetActive(true);
            }
        }

        public void MouseOverIcon(int index)
        {
            ItemDescPanelIcon.GetComponent<Image>().sprite = Inv.items[index].itemData.itemSprite;
            ItemDescPanelName.GetComponent<Text>().text = Inv.items[index].itemData.itemName;
            ItemDescPanelDesc.GetComponent<Text>().text = Inv.items[index].itemData.tooltip;
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


        IEnumerator Wait(int second)
        {
            yield return new WaitForSeconds(second);
        }
    }
}
