using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Item
{
    [CreateAssetMenu(fileName = "DataCard", menuName = "SO/Data - Item/DataCard")]
    public class Item_DataCardSO : AItemDataSO
    {
        public int increAmount;

        public override AItem CreateItem()
        {
            return new Item_DataCard(this);
        }
    }
}

