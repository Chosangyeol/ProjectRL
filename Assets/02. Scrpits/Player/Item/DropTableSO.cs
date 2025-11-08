using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DropTable", menuName = "SO/Data - DropTable")]
public class DropTableSO : ScriptableObject
{
    [System.Serializable]
    public class DropItem
    {
        public PoolableMono item;
        [Range(0f, 100f)]
        public float weight;
    }

    [System.Serializable]
    public class RarityGroup
    {
        public Enums.ItemRarity rarity;
        [Range(0f, 100f)]
        public float rarityWeight = 25f;
        public List<DropItem> items;
    }

    [Header("등급별 아이템 그룹")]
    public List<RarityGroup> rarityGroup;
}
