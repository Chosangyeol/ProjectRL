using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool : MonoBehaviour
{
    public PoolingListSO itemList;
    public PoolableMono test;

    private void Start()
    {
        itemList.PoolList.ForEach(p =>
        {
            PoolManager.Instance.CreatePool(p.Prefab,p.Count);
        });
    }
}
