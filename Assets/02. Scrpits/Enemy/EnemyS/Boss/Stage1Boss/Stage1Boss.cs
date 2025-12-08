using Info;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Boss : BossBase
{
    public GameObject pattern2Projectile;
    public GameObject pattern3Projectile;
    public Transform firePos;
    public Transform missilePos;
    public GameObject pattern3Warning;

    public AudioClip Pattern1Clip;
    public AudioClip Pattern3FireClip;
    public AudioClip Pattern3BoomClip;

#pragma warning disable CS0114 // 멤버가 상속된 멤버를 숨깁니다. override 키워드가 없습니다.
    private void Start()
#pragma warning restore CS0114 // 멤버가 상속된 멤버를 숨깁니다. override 키워드가 없습니다.
    {
        attackBehavior = new Stage1BossAttack(pattern2Projectile, pattern3Projectile,firePos, missilePos, pattern3Warning);

        // 레이저 세팅
        lr = GetComponent<LineRenderer>();      
        lr.enabled = false;
    }

    private void Update()
    {
        CheckLaser();
        fsm.Tick();
    }

    private void CheckLaser()
    {
        if (!isSpecialPattern || isSpinning) return;
        if (!(attackBehavior is Stage1BossAttack bossAttack)) return;
        if (bossAttack.isAttacking) return;

        fsm.ChangeState(new State_BossSpecialPattern(this, fsm, patternCount));
        StartCoroutine(LaserSpin());
    }

    public override void TakeDamage(int amount)
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
            isSpecialPattern = true;
            Debug.Log("레이저 공격");         
        }
        if (Stat.curHp <= 0 && !isDie)
        {
            isDie = true;
            StopAllCoroutines();
            PlaySound(deathClip);

            fsm.ChangeState(new State_Die(this, fsm));

            PlayerModel model = player.GetComponentInChildren<PlayerModel>();

            model.Stat.AddExp(enemySO.gainExp);
            FindAnyObjectByType<MainUIManager>().UpdateExp(model);

            TryBossDrop(enemySO.itemDropTable);

            Die();
            Anim.SetTrigger("Die");
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
    public GameObject laserPos;
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
        lr.positionCount = 2;
        lr.startWidth = laserWarningWidth;
        lr.endWidth = laserWarningWidth;
        lr.SetPosition(0, laserPos.transform.position);
        lr.SetPosition(1, laserPos.transform.position + transform.forward * laserRange);

        lr.enabled = true;
        isSpinning = true;


        yield return new WaitForSeconds(5f);

        lr.startWidth = laserWidth;
        lr.endWidth = laserWidth;

        float duringTime = 0f;
        

        while (duringTime < spinDuration)
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
            LaserFire();
            duringTime += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        lr.enabled = false;
        isSpecialPattern = false;
        yield return new WaitForSeconds(3f);
        fsm.ChangeState(new State_BossChase(this, fsm, patternCount));

    }

    private void LaserFire()
    {
        Vector3 pos = laserPos.transform.position;
        Vector3 dir = laserPos.transform.forward;

        RaycastHit hit;

        bool rayBlocked = Physics.Raycast(pos, dir, out hit, laserRange);

        if (rayBlocked)
        {
            lr.SetPosition(0, pos);
            lr.SetPosition(1, hit.point);             
        }
        else
        {
            lr.SetPosition(0, pos);
            lr.SetPosition(1, pos + dir * laserRange);
        }

        float castDist = rayBlocked ? hit.distance : laserRange;

        Vector3 box = new Vector3(laserWidth/2, laserWidth/2, laserWidth / 2);
        RaycastHit boxHit;

        if (Physics.BoxCast(pos,box,dir,out boxHit, laserPos.transform.rotation, castDist))
        {
            PlayerModel player = boxHit.collider.GetComponent<PlayerModel>();
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
    }
    #endregion

}
