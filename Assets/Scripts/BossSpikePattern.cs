using UnityEngine;
using System.Collections;
using UnityEditor.Experimental.GraphView;

public class BossSpikePattern : MonoBehaviour
{
    [Header("Boss Movement Settings")]
    public float riseY = 10f;        // 보스를 올릴 Y 좌표
    public float moveSpeed = 5f;     // 보스 이동 속도
    private Vector3 originalPos;     // 원래 위치 저장

    [Header("Spike Settings")]
    public GameObject spikePrefab;   // 스파이크 프리팹
    public int spikeCount = 20;      // 생성할 개수
    public float spikeDropHeight = 15f; // 땅 기준 높이
    public float spikeDestroyDelay = 0.1f;

    private Collider groundCollider;
    private bool isPatternRunning = false;

    void Start()
    {
        originalPos = transform.position;

        // Ground 찾기
        GameObject groundObj = GameObject.FindGameObjectWithTag("Ground");
        if (groundObj)
        {
            groundCollider = groundObj.GetComponent<Collider>();
        }
        else
        {
            Debug.LogError("Ground 태그 오브젝트를 찾을 수 없습니다.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isPatternRunning)
        {
            StartCoroutine(BossPattern());
        }
    }

    IEnumerator BossPattern()
    {
        isPatternRunning = true;

        // 1) 보스 상승
        Vector3 targetPos = new Vector3(originalPos.x, riseY, originalPos.z);
        yield return StartCoroutine(MoveBoss(targetPos));

        // 2) 스파이크 떨어뜨리기
        yield return StartCoroutine(DropSpikes());

        // 3) 원래 자리로 복귀
        yield return StartCoroutine(MoveBoss(originalPos));

        isPatternRunning = false;
    }

    IEnumerator MoveBoss(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator DropSpikes()
    {
        if (groundCollider == null)
            yield break;

        Bounds groundBounds = groundCollider.bounds;

        for (int i = 0; i < spikeCount; i++)
        {
            float randX = Random.Range(groundBounds.min.x, groundBounds.max.x);
            float randZ = Random.Range(groundBounds.min.z, groundBounds.max.z);

            Vector3 spawnPos = new Vector3(
                randX,
                groundBounds.max.y + spikeDropHeight,
                randZ
            );

            GameObject spike = Instantiate(spikePrefab, spawnPos, Quaternion.identity);

            // Rigidbody 없으면 자동 추가
            Rigidbody rb = spike.GetComponent<Rigidbody>();
            if (rb == null)
                rb = spike.AddComponent<Rigidbody>();

            rb.useGravity = true;
            rb.isKinematic = false;

            // 충돌 감지 + 자동 삭제 스크립트 추가
            SpikeAutoDestroy destroyer = spike.AddComponent<SpikeAutoDestroy>();
            destroyer.groundTag = "Ground";
            destroyer.lifeTime = 5f;    // 5초 뒤 자동 삭제

            yield return new WaitForSeconds(0.1f);
        }
    }
}


// ================================
// 스파이크 자동 삭제 스크립트
// ================================
public class SpikeAutoDestroy : MonoBehaviour
{
    public string groundTag = "Ground";
    public float lifeTime = 5f;  //   5초 뒤 자동 삭제

    void Start()
    {
        // 생성 시 5초 뒤에 삭제 예약
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(groundTag))
        {
            Destroy(gameObject);
        }
    }
}
