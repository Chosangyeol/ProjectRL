using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using Random = UnityEngine.Random;

public class EnemyDirector : MonoBehaviour
{
    [SerializeField]
    private PoolingListSO spawnEnemyList;
    public float interval = 60f;

    private Transform player;

    public List<Transform> spawnPosList;
    private List<Transform> lastSpawnPos = new List<Transform>();
    private int maxRecentCount = 2;

    public int enemyCount = 0;
    [Header("스테이지 별 최대 몬스터 수")]
    public int maxEnemyCount = 20;
    private int enemyKillCount = 0;
    public int EnemyKillCount => enemyKillCount;

    public int stageIndex = 1;

    [Header("보스 메뉴")]
    private bool bossOpen = false;
    public PoolableMono boss;
    public Transform bossPos;
    public GameObject bossDoor;
    public int openCount = 20;
    public GameObject portal;
    public PlayableDirector pd;
    private bool isPlayed = false;
    private MainUIManager mainUIManager;

    [Header("포탑 메뉴")]
    public PoolableMono[] towerPrefab;
    public int maxTowerCount = 3;

    [Header("Bgm 변경")]
    public BGMPlayer bgmPlayer;

    public event Action OnBossOpen;


    #region Unity Events

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // 자동 생성을 시작할 시간대 조절 해야함. 1f -> 60f = 1분후 플레이어 주변 스폰
        boss.gameObject.SetActive(false);
        mainUIManager = FindAnyObjectByType<MainUIManager>();
        while (enemyCount < maxEnemyCount)
        {
            TrySpawn();
        }
        SetTower();
        portal.SetActive(false);
        InvokeRepeating(nameof(TrySpawn), 60f, interval);
    }
    #endregion

    #region Spawn
    void TrySpawn()
    {
        if (player == null || spawnEnemyList.PoolList.Count == 0) return;
        if (enemyCount >= maxEnemyCount) return;

        var spawnList = spawnEnemyList.PoolList;

        int totalWeight = spawnList.Sum(e =>
            e.Prefab.GetComponent<EnemyBase>().enemySO.weight[stageIndex - 1]
        );

        if (totalWeight <= 0) return;

        int randomValue = Random.Range(0, totalWeight);
        EnemyBase selectedEnemy = null;

        foreach (var e in spawnList)
        {
            var enemyBase = e.Prefab.GetComponent<EnemyBase>();
            int weight = enemyBase.enemySO.weight[stageIndex - 1];

            if (randomValue < weight)
            {
                selectedEnemy = enemyBase;
                break;
            }

            randomValue -= weight;
        }

        if (selectedEnemy == null) return;

        SpawnEnemy(selectedEnemy, selectedEnemy.enemySO.spawnCount);
    }

    Transform GetSpawnPosition()
    {
        // 스폰 포인트 후보
        List<Transform> candidates = new List<Transform>(spawnPosList);

        // 최근 스폰했던 포인트 2개 제거
        foreach (var t in lastSpawnPos)
        {
            candidates.Remove(t);
        }

        // 만약 후보가 없다 → 모든 가능 포인트를 사용할 수밖에 없음
        if (candidates.Count == 0)
        {
            candidates = new List<Transform>(spawnPosList);
        }

        // 랜덤 포인트 선택
        Transform selected = candidates[Random.Range(0, candidates.Count)];

        // 최근 스폰 리스트 갱신
        lastSpawnPos.Add(selected);

        // 최근 스폰 리스트 최대 2개 유지
        if (lastSpawnPos.Count > maxRecentCount)
            lastSpawnPos.RemoveAt(0); // 가장 오래된 값 제거

        return selected;
    }

    void SpawnEnemy(EnemyBase enemyPrefab, int count)
    {
        Transform spawnPos = GetSpawnPosition();

        for (int i = 0; i < count; i++)
        {
            Vector2 offset = Random.insideUnitCircle * 2f;
            Vector3 spanwPos = spawnPos.position + new Vector3(offset.x, player.position.y + 1, offset.y);

            EnemyBase enemy = PoolManager.Instance.Pop(enemyPrefab.gameObject.name) as EnemyBase;
            enemy.transform.position = spanwPos;
            if (enemy.Agent != null)
                enemy.Agent.Warp(spanwPos);

            enemyCount++;
        }
    }
    #endregion

    #region Boss Setting
    public void IncreKillCount()
    {
        enemyKillCount += 1;
        if (enemyKillCount >= openCount && !bossOpen)
        {
            bossOpen = true;
            interval = 120f;
            maxEnemyCount = maxEnemyCount / 2;
            CancelInvoke(nameof(TrySpawn));
            InvokeRepeating(nameof(TrySpawn), 0f, interval);
            Debug.Log("보스방 오픈");
            bossDoor.SetActive(false);
            OnBossOpen?.Invoke();
        }
    }

    public void StartBossIntroCutScene()
    {
        isPlayed = true;
        mainUIManager.gameObject.SetActive(false);
        if (!pd.gameObject.activeSelf) pd.gameObject.SetActive(true);
        pd.stopped += SpawnBoss;
        bgmPlayer.BossBgmPlay();
        pd.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPlayed)
        {
            bossDoor.SetActive(true);
            Debug.Log("보스 컷씬 작동");
            StartBossIntroCutScene();

        }
    }

    public void SpawnBoss(PlayableDirector obj)
    {
        PoolableMono bossObj = PoolManager.Instance.Pop(boss.name);

        var agent = bossObj.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = false;
        }

        // 원하는 위치로 이동
        bossObj.transform.position = bossPos.position;
        bossObj.transform.rotation = bossPos.rotation;

        // NavMeshAgent 다시 활성화
        if (agent != null)
        {
            agent.Warp(bossPos.position);
            agent.enabled = true;
        }
        mainUIManager.gameObject.SetActive(true);
    }

    public void OpenPortal()
    {
        portal.SetActive(true);
    }

    #endregion

    #region Tower Setting
    private void SetTower()
    {
        List<Transform> towerPos = spawnPosList;

        for (int i = 0; i < maxTowerCount; i++)
        {
            int posIndex = Random.Range(0,towerPos.Count);
            Vector3 towerSpawnPos = towerPos[posIndex].position;
            towerPos.Remove(towerPos[posIndex]);

            RaycastHit ray;

            if (Physics.Raycast(towerSpawnPos, Vector3.down, out ray, 100f, LayerMask.GetMask("Ground")))
            {
                towerSpawnPos = ray.point;

                towerSpawnPos.x += Random.Range(-5, 5);
                towerSpawnPos.z += Random.Range(-5, 5);

                int towerIndex = Random.Range(0, towerPrefab.Length);

                PoolableMono tower = PoolManager.Instance.Pop(towerPrefab[towerIndex].name);
                tower.transform.position = towerSpawnPos;
            }        
        }
    }

    #endregion
}
