using Info;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Boss : BossBase
{
    public GameObject pattern2Projectile;
    public Transform firePos;

#pragma warning disable CS0114 // 멤버가 상속된 멤버를 숨깁니다. override 키워드가 없습니다.
    private void Start()
#pragma warning restore CS0114 // 멤버가 상속된 멤버를 숨깁니다. override 키워드가 없습니다.
    {
        attackBehavior = new Stage1BossAttack(pattern2Projectile,firePos);

        // 레이저 세팅
        lr = GetComponent<LineRenderer>();      
        lr.enabled = false;

 
    }

    public override void TakeDamage(float amount)
    {
        Stat.curHp -= amount;
        if (Stat.curHp < (Stat.totalHp * 0.8f) && !isFirstDrop)
        {
            Debug.Log("첫번째 낙석");
            isFirstDrop = true;
            StartDrop();
        }
        if (Stat.curHp < (Stat.totalHp * 0.7f) && !isSecondDrop)
        {
            Debug.Log("두번째 낙석");
            isSecondDrop = true;
            StartDrop();
        }
        if (Stat.curHp < (Stat.totalHp * 0.5f) && !isSpinning)
        {
            Debug.Log("레이저 공격");
            StopCoroutine(attackCoroutine);
            fsm.ChangeState(new State_BossSpecialPattern(this, fsm, patternCount));
            StartCoroutine(LaserSpin());
        }

    }

    #region Boss Drop Wall

    private bool isFirstDrop = false;
    private bool isSecondDrop = false;

    [Header("보스 낙석 설정")]
    [Tooltip("낙석 경고 Prefab")]
    public GameObject warningPrefab;
    [Tooltip("낙석 Prefab")]
    public GameObject fallingPrefab;
    [Tooltip("낙석 경고 시간 ( 낙석 지연 시간 )")]
    public float warningDuration = 2f;
    [Tooltip("낙석 생성 높이")]
    public float fallHeight = 20f;
    [Tooltip("낙석 속도")]
    public float fallSpeed = 25f;

    private void StartDrop()
    {
        if (player == null) return;

        Vector3 pos = player.transform.position + new Vector3(0, 0.1f, 0);

        StartCoroutine(DropActive(pos));
    }

    private IEnumerator DropActive(Vector3 pos)
    {
        GameObject warning = Instantiate(warningPrefab, pos, Quaternion.identity);

        yield return new WaitForSeconds(warningDuration);

        Vector3 spawnPos = pos + Vector3.up * fallHeight;
        GameObject falling = Instantiate(fallingPrefab, spawnPos, Quaternion.identity);

        Rigidbody rb = falling.GetComponent<Rigidbody>();
        if (rb == null) rb = falling.AddComponent<Rigidbody>();

        rb.useGravity = false;

        while (falling.transform.position.y > pos.y + 0.5f)
        {
            falling.transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            yield return null;
        }

        falling.transform.position = new Vector3(
            pos.x,
            pos.y,
            pos.z
        );

        Destroy(rb);
        falling.transform.rotation = Quaternion.identity;

        Destroy(warning);
    }

    #endregion

    #region Boss Laser
    [Header("레이저 공격 설정")]
    [Tooltip("회전 속도")]
    public float rotationSpeed = 180f;
    [Tooltip("레이저 사거리")]
    public float laserRange = 30f;
    [Tooltip("레이저 색상")]
    // 추후 레이저 생성 방식 변경할수도.
    public Color laserColor = Color.red;
    [Tooltip("레이저 두께")]
    public float laserWidth = 0.05f;
    private float laserWarningWidth = 0.2f;
    [Tooltip("레이저 지속시간")]
    public float spinDuration = 10f;

    private bool isSpinning = false;

    private IEnumerator LaserSpin()
    {
        lr.material = new Material(Shader.Find("Unlit/Color"));
        lr.material.color = laserColor;
        lr.positionCount = 2;
        lr.startWidth = laserWarningWidth;
        lr.endWidth = laserWarningWidth;
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position + transform.forward * laserRange);

        lr.enabled = true;


        yield return new WaitForSeconds(5f);

        lr.startWidth = laserWidth;
        lr.endWidth = laserWidth;

        float duringTime = 0f;
        isSpinning = true;
        

        while (duringTime < spinDuration)
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
            LaserFire();
            duringTime += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        lr.enabled = false;

        yield return new WaitForSeconds(3f);
        fsm.ChangeState(new State_BossChase(this, fsm, patternCount));

    }

    private void LaserFire()
    {
        Debug.Log("회전 중");
        Vector3 pos = transform.position;
        Vector3 dir = transform.forward;

        Ray ray = new Ray(pos, dir);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, laserRange))
        {
            lr.SetPosition(0, pos);
            lr.SetPosition(1, hit.point);

            PlayerModel player = hit.collider.GetComponent<PlayerModel>();
            if (player != null)
            {
                SInfoAttack laserDamage = new SInfoAttack(
                    this.gameObject,
                    player.gameObject,
                    Mathf.RoundToInt(Stat.totalDamage * 1.5f),
                    null
                );

                player.Damaged(laserDamage);
            }
                
        }
        else
        {
            lr.SetPosition(0, pos);
            lr.SetPosition(1, pos + dir * laserRange);
        }
    }
    #endregion

}
