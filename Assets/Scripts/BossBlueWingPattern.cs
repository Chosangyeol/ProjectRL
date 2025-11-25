using UnityEngine;
using System.Collections;

public class BossBlueWingPattern : MonoBehaviour
{
    // 패턴 재사용 방지
    private bool isRunning = false;

    // 보스 색상 설정
    public Renderer bossRenderer;
    public Color alertColor = Color.blue;
    private Color originalColor;
    public float colorReturnDelay = 5f;

    // 오브젝트 설정
    public GameObject wingPrefab;
    public int spawnCount = 50;
    public float spawnAreaX = 10f;
    public float spawnAreaY = 5f;
    public float spawnHeight = 10f;
    public float moveSpeed = 20f;
    public float destroyDelay = 7f;

    // 플레이어
    public Transform player;

    void Start()
    {
        if (bossRenderer == null)
            bossRenderer = GetComponentInChildren<Renderer>();

        originalColor = bossRenderer.material.color;
    }

    void Update()
    {
        // E 키로 발동
        if (Input.GetKeyDown(KeyCode.C) && !isRunning)
        {
            StartCoroutine(PatternRoutine());
        }
    }

    IEnumerator PatternRoutine()
    {
        isRunning = true;

        // 즉시 파란색으로 변경
        bossRenderer.material.color = alertColor;

        // 3초 후 공격 발사
        yield return new WaitForSeconds(3f);

        yield return StartCoroutine(SpawnWingsRoutine());

        // 남은 시간 지나면 색 복귀
        yield return new WaitForSeconds(colorReturnDelay - 3f);

        bossRenderer.material.color = originalColor;

        isRunning = false;
    }

    IEnumerator SpawnWingsRoutine()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            // 랜덤 위치 생성
            float randX = Random.Range(-spawnAreaX, spawnAreaX);
            float randY = Random.Range(0f, spawnAreaY);

            Vector3 spawnPos = new Vector3(
                transform.position.x + randX,
                transform.position.y + spawnHeight + randY,
                transform.position.z
            );

            // 오브젝트 생성
            GameObject obj = Instantiate(wingPrefab, spawnPos, Quaternion.identity);

            // 자동 파괴 예약
            Destroy(obj, destroyDelay);

            // 플레이어 방향 바라보면서 바로 이동 처리
            StartCoroutine(MoveTowardPlayer(obj));

            yield return null;
        }
    }

    IEnumerator MoveTowardPlayer(GameObject obj)
    {
        while (obj != null)
        {
            if (player != null)
            {
                // 방향 계산
                Vector3 dir = (player.position - obj.transform.position).normalized;

                // 바라보기
                obj.transform.rotation = Quaternion.LookRotation(dir);

                // 플레이어 쪽으로 이동
                obj.transform.position += obj.transform.forward * moveSpeed * Time.deltaTime;
            }

            yield return null;
        }
    }
}