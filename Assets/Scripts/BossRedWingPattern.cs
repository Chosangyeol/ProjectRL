using UnityEngine;
using System.Collections;

public class BossRedWingPattern : MonoBehaviour
{
    // 보스가 패턴을 사용하는 동안 다시 사용되지 않도록 막는 변수
    private bool isRunning = false;

    // 보스 이동 관련 변수
    [Header("Boss Movement Settings")]
    public float bossRiseY = 2f;           // 보스가 상승하는 높이
    public float bossMoveSpeed = 3f;       // 보스가 이동하는 속도
    private Vector3 bossOriginalPos;       // 보스의 원래 위치 저장

    // 보스 색상 관련 변수
    [Header("Boss Color Settings")]
    public Renderer bossRenderer;          // 보스의 렌더러
    public Color alertColor = Color.red;   // 경고 색 즉시 빨간색
    private Color originalColor;           // 원래 색 저장
    public float colorReturnDelay = 5f;    // 빨간색 유지 시간

    // 연결된 오브젝트 관련 변수
    [Header("Connected Object Settings")]
    public Transform connectedObject;      // 이동해야 하는 오브젝트
    public float targetY = 10f;            // 오브젝트가 올라갈 목표 y값
    public float moveDuration = 3f;        // 올라가는데 걸리는 시간과 내려오는데 걸리는 시간

    private Vector3 connectedOriginalPos;  // 오브젝트 원래 위치 저장

    void Start()
    {
        // 보스의 처음 위치 저장
        bossOriginalPos = transform.position;

        // 렌더러가 지정되지 않았으면 자식에서 찾아서 저장
        if (bossRenderer == null)
            bossRenderer = GetComponentInChildren<Renderer>();

        // 원래 색 저장
        originalColor = bossRenderer.material.color;

        // 연결된 오브젝트 원래 위치 저장
        if (connectedObject != null)
            connectedOriginalPos = connectedObject.position;
    }

    void Update()
    {
        // w를 눌렀을 때 패턴이 실행 중이 아니라면 실행
        if (Input.GetKeyDown(KeyCode.X) && !isRunning)
        {
            StartCoroutine(BossPatternRoutine());
        }
    }

    IEnumerator BossPatternRoutine()
    {
        // 패턴이 실행 중으로 표시하여 재사용 방지
        isRunning = true;

        // 1 보스 색 즉시 빨간색으로 변경
        bossRenderer.material.color = alertColor;

        // 2 보스를 상승시키기 시작
        Vector3 risePos = new Vector3(
            bossOriginalPos.x,
            bossOriginalPos.y + bossRiseY,
            bossOriginalPos.z
        );
        StartCoroutine(MoveBoss(risePos));

        // 3 빨개진 상태에서 3초 동안 대기
        yield return new WaitForSeconds(3f);

        // 4 연결된 오브젝트 상승 하강 코루틴 실행
        yield return StartCoroutine(LiftObjectRoutine());

        // 5 보스를 원래 위치로 되돌림
        yield return StartCoroutine(MoveBoss(bossOriginalPos));

        // 6 오브젝트 동작이 끝났어도 색은 총 5초 유지해야 하므로 남은 시간 계산
        float remaining = colorReturnDelay - 3f;
        if (remaining > 0)
            yield return new WaitForSeconds(remaining);

        // 7 색을 원래 색으로 복귀
        bossRenderer.material.color = originalColor;

        // 8 패턴 사용 가능 상태로 되돌림
        isRunning = false;
    }

    IEnumerator MoveBoss(Vector3 target)
    {
        // 보스가 목표 위치에 가까워질 때까지 계속 이동
        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                Time.deltaTime * bossMoveSpeed
            );
            yield return null;
        }
    }

    IEnumerator LiftObjectRoutine()
    {
        // 오브젝트가 올라갈 위치 계산
        Vector3 upPos = new Vector3(
            connectedOriginalPos.x,
            targetY,
            connectedOriginalPos.z
        );

        // 3초 상승
        yield return StartCoroutine(MoveObject(connectedObject, connectedOriginalPos, upPos, moveDuration));

        // 3초 하강
        yield return StartCoroutine(MoveObject(connectedObject, upPos, connectedOriginalPos, moveDuration));
    }

    IEnumerator MoveObject(Transform obj, Vector3 start, Vector3 end, float duration)
    {
        // 시간 계산용 변수
        float t = 0f;

        // 정해진 시간 동안 선형 보간으로 위치 이동
        while (t < duration)
        {
            t += Time.deltaTime;
            float normalized = Mathf.Clamp01(t / duration);

            obj.position = Vector3.Lerp(start, end, normalized);

            yield return null;
        }
    }
}