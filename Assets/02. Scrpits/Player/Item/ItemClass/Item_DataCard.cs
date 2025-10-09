using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Item
{
    public class Item_DataCard : AItem
    {
        private Item_DataCardSO data;

        public Item_DataCard(Item_DataCardSO itemData) : base(itemData)
        {
            data = itemData;
        }

        public override void OnAddInventory(PlayerModel model)
        {
            Debug.Log("데이터 칩 (" + data.itemName + ") 추가됨");
        }

        public override bool OnUpdateInventory(PlayerModel model, float delta)
        {
            return true;
        }

        public override void OnRemoveInventory(PlayerModel model)
        {
            Debug.Log("데이터 칩 (" + data.itemName + ") 삭제됨");
        }
    }
}

