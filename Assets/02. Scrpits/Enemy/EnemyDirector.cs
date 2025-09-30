using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDirector : MonoBehaviour
{
    [SerializeField]
    private PoolingListSO EnemyList;

    private void Awake()
    {
        CreateEnemyPool();
    }

    private void CreateEnemyPool()
    {
        PoolManager.Instance = new PoolManager(transform);
        EnemyList.PoolList.ForEach(p =>
        {
            PoolManager.Instance.CreatePool(p.Prefab, p.Count);
        });
    }
}
