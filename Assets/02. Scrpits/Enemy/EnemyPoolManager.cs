using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour
{
    [SerializeField]
    private PoolingListSO EnemyList;
    [SerializeField]
    private PoolingListSO ProjectileList;
    [SerializeField]
    private PoolingListSO itemList;
    [SerializeField]
    private PoolingListSO towerList;

    private void Awake()
    {
        CreatePool();
    }

    private void CreatePool()
    {

        EnemyList.PoolList.ForEach(p =>
        {
            PoolManager.Instance.CreatePool(p.Prefab, p.Count);
        });

        ProjectileList.PoolList.ForEach(p =>
        {
            PoolManager.Instance.CreatePool(p.Prefab, p.Count);
        });

        itemList.PoolList.ForEach(p =>
        {
            PoolManager.Instance.CreatePool(p.Prefab, p.Count);
        });

        towerList.PoolList.ForEach(p =>
        {
            PoolManager.Instance.CreatePool(p.Prefab, p.Count);
        });
    }
}
