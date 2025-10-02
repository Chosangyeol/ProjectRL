using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyDirector : MonoBehaviour
{
    [SerializeField]
    private PoolingListSO EnemyList;
    [SerializeField]
    private PoolingListSO ProjectileList;
    public float spawnRadius = 15f;
    public float interval = 10f;

    public float cost = 0f;
    public float costPerSeconds = 1f;
    public float timer = 0f;

    private Transform player;


    #region Unity Events
    private void Awake()
    {
        CreateEnemyPool();
        ProjectileList.PoolList.ForEach(p =>
        {
            PoolManager.Instance.CreatePool(p.Prefab, p.Count);
        });
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // �ڵ� ������ ������ �ð��� ���� �ؾ���. 1f -> 60f = 1���� �÷��̾� �ֺ� ����
        InvokeRepeating(nameof(TrySpawn), 1f, interval);
    }
    void Update()
    {
        timer += Time.deltaTime;
        // ���̵� ��¿� ���� ���� ��ȯ �ڽ�Ʈ ������ ���� �ʿ�
        // �ð� ���̵� + ���� ���̵� �ݿ�
        cost += costPerSeconds;
    }
    #endregion

    private void CreateEnemyPool()
    {
        PoolManager.Instance = new PoolManager(transform);
        EnemyList.PoolList.ForEach(p =>
        {
            PoolManager.Instance.CreatePool(p.Prefab, p.Count);
        });
    }

    #region Spawn
    void TrySpawn()
    {
        if (player == null || EnemyList.PoolList.Count == 0) return;

        // EnemyList Ǯ���� ���� Cost�� �����Ǵ� ���� ����Ʈ ����
        var canSpawnEnemies = EnemyList.PoolList.Where(e => e.Prefab.GetComponent<EnemyBase>().enemySO.cost <= cost).ToList();
        if (canSpawnEnemies.Count == 0) return;

        int totlaWeight = canSpawnEnemies.Sum(e => e.Prefab.GetComponent<EnemyBase>().enemySO.weight);
        int randomValue = Random.Range(0, totlaWeight);
        EnemyBase selectedEnemy = null;

        foreach (var e in canSpawnEnemies)
        {
            int weight = e.Prefab.GetComponent<EnemyBase>().enemySO.weight;
            if (randomValue < e.Prefab.GetComponent<EnemyBase>().enemySO.weight)
            {
                selectedEnemy = e.Prefab.GetComponent<EnemyBase>();
                break;
            }
            randomValue -= weight;
        }
        if (selectedEnemy == null) return;
        cost -= selectedEnemy.enemySO.cost;

        SpawnEnemy(selectedEnemy, selectedEnemy.enemySO.spawnCount);
    }

    void SpawnEnemy(EnemyBase enemyPrefab, int count)
    {
        Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius; ;
        Vector3 spawnPosition = player.position + new Vector3(randomCircle.x, 0, randomCircle.y);

        for (int i = 0; i < count; i++)
        {
            Vector2 offset = Random.insideUnitCircle * 2f;
            Vector3 spanwPos = spawnPosition + new Vector3(offset.x, player.position.y+1, offset.y);

            EnemyBase enemy = PoolManager.Instance.Pop(enemyPrefab.gameObject.name) as EnemyBase;
            enemy.gameObject.transform.position = spanwPos;
            enemy.agent.Warp(spanwPos);
        }
    }
    #endregion
}
