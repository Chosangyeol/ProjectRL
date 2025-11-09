using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Item
{
    [CreateAssetMenu(fileName = "DataCard", menuName = "SO/Data - Item/DataCard")]
    public class Item_DataCardSO : AItemDataSO
    {
        public int increAmount;
        public CanIncreStats canIncreStats;

        public override AItem CreateItem()
        {
            return new Item_DataCard(this);
        }
    }

    public enum CanIncreStats
    {
        hpMax = 0,
        hpRegenPerSecond,
        speedMove,
        attackDamage,
        critPercent,
        critDamagePercent
    }
}

