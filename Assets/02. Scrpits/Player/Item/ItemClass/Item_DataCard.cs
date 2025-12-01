using Player.Component;
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
            model.Stat.AddStat(SetPlayerStat());
            if (data.canIncreStats == CanIncreStats.hpMax) { model.Stat.Healed(data.increAmount); }
            Debug.Log("변화 스탯 : " + data.canIncreStats + " / 증가량 : " + data.increAmount);
        }

        public override bool OnUpdateInventory(PlayerModel model, float delta)
        {
            return true;
        }

        public override void OnRemoveInventory(PlayerModel model)
        {
            Debug.Log("데이터 칩 (" + data.itemName + ") 삭제됨");
            model.Stat.AddStat(SetPlayerStat(true));
            if (data.canIncreStats == CanIncreStats.hpMax) { model.Stat.Healed(-data.increAmount); }
            
        }

        private SPlayerStat SetPlayerStat(bool reverse = false)
        {
            float value = reverse ? -data.increAmount : data.increAmount;

            if (data.canIncreStats == CanIncreStats.hpMax)
            {
                return new SPlayerStat { hpMax = Mathf.RoundToInt(value) };
            }
            else if (data.canIncreStats == CanIncreStats.hpRegenPerSecond)
            {
                return new SPlayerStat { hpRegenPerSecond = Mathf.RoundToInt(value) };
            }
            else if (data.canIncreStats == CanIncreStats.speedMove)
            {
                return new SPlayerStat { speedMove = value };
            }
            else if (data.canIncreStats == CanIncreStats.attackDamage)
            {
                return new SPlayerStat { attackDamage = Mathf.RoundToInt(value) };
            }
            else if (data.canIncreStats == CanIncreStats.critPercent)
            {
                value /= 100;
                return new SPlayerStat { critPercent = value };
            }
            else if (data.canIncreStats == CanIncreStats.critDamagePercent)
            {
                value /= 100;
                return new SPlayerStat { critDamagePercent = value };
            }
            else
            {
                return new SPlayerStat();
            }
        }
    }
}

