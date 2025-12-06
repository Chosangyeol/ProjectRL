using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour
{
    [SerializeField]
    private PoolingListSO EnemyList;
    [SerializeField]
    private PoolingListSO ProjectileList;

    private void Awake()
    {
        CreateEnemyPool();
    }

    private void CreateEnemyPool()
    {

        EnemyList.PoolList.ForEach(p =>
        {
            PoolManager.Instance.CreatePool(p.Prefab, p.Count);
        });

        ProjectileList.PoolList.ForEach(p =>
        {
            PoolManager.Instance.CreatePool(p.Prefab, p.Count);
        });
    }
}
