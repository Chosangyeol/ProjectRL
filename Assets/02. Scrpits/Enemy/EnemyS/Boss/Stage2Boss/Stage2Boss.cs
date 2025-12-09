using DG.Tweening.Core.Easing;
using Info;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Boss : BossBase
{
    [Header("일반 패턴")]
    public GameObject pattern3Warning;
    public GameObject pattern3Effect;

    public bool canRush = true;
    public float rushTimer = 0f;
    public GameObject rushEffect;
    public bool isHit = false;

    [Header("특수 패턴 1 - 독 늪")]
    public bool poisonTrigger = false;
    private int poisonIndex = 1;
    public PoolableMono poisonPref;
    public Transform poisonFIrePos;
    public float poisonSpeed = 1.0f;
    public bool isPoisonPlaying = false;

    [Header("특수 패턴 2 - 돌")]
    public Transform centorPos;
    public float jumpDur = 2f;
    public bool phase2 = false;
    public bool phase2Trigger = false;
    public bool isSpecialPlaying = false;
    public PoolableMono[] Rocks; 
    public Transform[] sequence1Targets;
    public GameObject[] sequence1Warning;
    public Transform[] sequence2Targets;
    public GameObject[] sequence2Warning;
    public Transform[] sequence3Targets;
    public GameObject[] sequence3Warning;

    [Header("보스 추가 사운드")]
    public AudioClip specialPattern2;

    private void Start()
    {
        if (agent != null)
            agent.speed = Stat.moveSpeed;

        attackBehavior = new Stage2BossAttack(pattern3Warning, pattern3Effect, rushEffect);
        StartCoroutine(CheckRush());
    }

    private void Update()
    {
        PoisonPattern();
        SpecialPattern();
        fsm.Tick();
    }

    public override void TakeDamage(int amount)
    {
        Stat.curHp -= amount;
        if (poisonIndex <= 9 && Stat.curHp <= (Stat.totalHp / 10) * (10 - poisonIndex))
        {
            if (Stat.curHp <= (Stat.totalHp * 0.5) && !phase2)
            {
                phase2 = true;
                phase2Trigger = true;
                poisonIndex++;
                return;
            }
            else
            {
                poisonTrigger = true;      
            }
        }

        if (Stat.curHp <= 0 && !isDie)
        {
            Die();
            Anim.SetTrigger("Die");
        }

    }

    public void IsAttackEnd(float delay)
    {
        if (attackBehavior is Stage2BossAttack boss)
        {
            boss.isAttacking = false;
            StartCoroutine(AttackDelay(delay));
        }
    }



    #region 특수 패턴 1 - 독 늪
    public void PoisonPattern()
    {
        if (!poisonTrigger) return;
        if (isSpecialPlaying) return;
        if (isPoisonPlaying) return;
        if (!(attackBehavior is Stage2BossAttack bossAttack)) return;
        if (bossAttack.isAttacking) return;

        fsm.ChangeState(new State_BossSpecialPattern(this, fsm, patternCount));
        Poison();
    }

    public void Poison()
    {
        isSpecialPattern = true;
        isPoisonPlaying = true;

        Vector3 dir = player.position - transform.position;
        dir.y = 0;
        transform.rotation = Quaternion.LookRotation(dir);


        anim.SetTrigger("Poison");
        poisonIndex++;
    }

    public void IsPoisonEnd()
    {
        poisonTrigger = false;
        isSpecialPattern = false;
        isPoisonPlaying = false;
        StartCoroutine(AttackDelay(2f));
    }

    public void ShotPoison()
    {
        PoolableMono obj = PoolManager.Instance.Pop(poisonPref.name);
        Projectile proj = obj.GetComponent<Projectile>();
        proj.owner = this;
        proj.damage = Stat.totalDamage;
        obj.transform.position = poisonFIrePos.position;
        Vector3 dir = (player.position - poisonFIrePos.position).normalized;
        obj.GetComponent<Rigidbody>().velocity = dir * poisonSpeed;
    }
    #endregion

    #region 특수 패턴 2 - 돌 던지기

    public void SpecialPattern()
    {
        if (!phase2) return;
        if (!phase2Trigger) return;
        if (isSpecialPlaying) return;
        if (isPoisonPlaying) return;
        if (!(attackBehavior is Stage2BossAttack bossAttack)) return;
        if (bossAttack.isAttacking) return;

        Debug.Log("트리거 발동");

        centorPos = GameObject.FindWithTag("Stage2BossCentor").transform;
        poisonTrigger = false;
        isPoisonPlaying = false;
        fsm.ChangeState(new State_BossSpecialPattern(this, fsm, patternCount));
        StartCoroutine(StartSpecialPattern());
    }

    IEnumerator StartSpecialPattern()
    {
        phase2Trigger = false;
        isSpecialPattern = true;
        isSpecialPlaying = true;
        anim.SetTrigger("Jump");
        yield return new WaitForSeconds(0.7f);
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + Vector3.up * 50;
        Debug.Log("점프");
        float t = 0f;
        while (t < jumpDur)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, t/jumpDur);
            yield return null;
        }

        t = 0f;
        transform.position = centorPos.position + Vector3.up * 50;
        startPos = transform.position;

        while (t < jumpDur)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, centorPos.position, t / jumpDur);
            yield return null;
        }
        Debug.Log("착지");

        anim.SetTrigger("Special2End");
        yield return new WaitForSeconds(1f);
        PatternSequence1();
        yield return new WaitForSeconds(3f);
        PatternSequence2();
        yield return new WaitForSeconds(3f);
        PatternSequence3();
        yield return new WaitForSeconds(7f);

        isSpecialPlaying = false;
        isSpecialPattern = false;
        
        StartCoroutine(AttackDelay(3f));

    }

    public void PatternSequence1()
    {
        int targetIndex = 0;
        StartCoroutine(ShowWarning(1));
        for (int i = 0; i < 4; i++)
        {
            int index = Random.Range(0, 4);
            PoolableMono rock = PoolManager.Instance.Pop(Rocks[index].name);
            Stage2Boss_Rock rockS = rock.GetComponent<Stage2Boss_Rock>();

            Vector3 dir = (sequence1Targets[targetIndex].position - transform.position).normalized;

            rock.transform.position = transform.position + (dir * 5) + (Vector3.up * 4);

            rockS.InitRock(sequence1Targets[targetIndex], 3f, this);
            rockS.rockSpeed = 30f;
            rockS.spinSpeed = 70f;
            rockS.Shot();
            targetIndex++;
        }
    }

    public void PatternSequence2()
    {
        int targetIndex = 0;
        StartCoroutine(ShowWarning(2));
        for (int i = 0; i < 4; i++)
        {
            int index = Random.Range(0, 4);
            PoolableMono rock = PoolManager.Instance.Pop(Rocks[index].name);
            Stage2Boss_Rock rockS = rock.GetComponent<Stage2Boss_Rock>();

            Vector3 dir = (sequence2Targets[targetIndex].position - transform.position).normalized;

            rock.transform.position = transform.position + (dir * 5) + (Vector3.up * 4);

            rockS.InitRock(sequence2Targets[targetIndex], 3f, this);
            rockS.rockSpeed = 30f;
            rockS.spinSpeed = 70f;
            rockS.Shot();
            targetIndex++;
        }
    }

    public void PatternSequence3()
    {
        int targetIndex = 0;
        StartCoroutine(ShowWarning(3));
        for (int i = 0; i < 12; i++)
        {
            int index = Random.Range(0, 4);
            PoolableMono rock = PoolManager.Instance.Pop(Rocks[index].name);
            Stage2Boss_Rock rockS = rock.GetComponent<Stage2Boss_Rock>();

            Vector3 dir = (sequence3Targets[targetIndex].position - transform.position).normalized;

            rock.transform.position = transform.position + (dir * 5) + (Vector3.up * 4);

            rockS.InitRock(sequence3Targets[targetIndex], 3f, this);
            rockS.rockSpeed = 30f;
            rockS.spinSpeed = 70f;
            rockS.Shot();
            targetIndex++;
        }
    }

    IEnumerator ShowWarning(int index)
    {
        if (index == 1)
        {
            for (int i = 0;i < sequence1Warning.Length;i++)
            {
                sequence1Warning[i].SetActive(true);
            }
            yield return new WaitForSeconds(2f);
            for (int i = 0; i < sequence1Warning.Length; i++)
            {
                sequence1Warning[i].SetActive(false);
            }
        }
        else if (index == 2)
        {
            for (int i = 0; i < sequence2Warning.Length; i++)
            {
                sequence2Warning[i].SetActive(true);
            }
            yield return new WaitForSeconds(2f);
            for (int i = 0; i < sequence2Warning.Length; i++)
            {
                sequence2Warning[i].SetActive(false);
            }
        }
        else if (index == 3)
        {
            for (int i = 0; i < sequence3Warning.Length; i++)
            {
                sequence3Warning[i].SetActive(true);
            }
            yield return new WaitForSeconds(2f);
            for (int i = 0; i < sequence3Warning.Length; i++)
            {
                sequence3Warning[i].SetActive(false);
            }
        }
    }


    #endregion

    #region 일반 패턴 1 - 전방 베기

    public void Pattern1Attack()
    {
        Vector3 centor = transform.position + transform.forward * 5 + transform.up * 5;
        Vector3 size = new Vector3(5, 5, 5);
        Collider[] hits = Physics.OverlapBox(centor, size, Quaternion.identity);

        foreach (Collider col in hits)
        {
            if (col.CompareTag("Player"))
            {
                SInfoAttack damamge = new SInfoAttack(
                    this.gameObject,
                    player.gameObject,
                    Mathf.RoundToInt(Stat.totalDamage),
                    null
                    );

                PlayerModel model = col.GetComponentInChildren<PlayerModel>();
                model.Damaged(damamge);

            }
        }
    }

    #endregion

    #region 일반 패턴 2 - 회오리

    public void Pattern2Attack()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 10f);

        foreach (Collider col in hits)
        {
            if (col.CompareTag("Player"))
            {
                SInfoAttack damamge = new SInfoAttack(
                    this.gameObject,
                    player.gameObject,
                    Mathf.RoundToInt(Stat.totalDamage),
                    null
                    );

                PlayerModel model = col.GetComponentInChildren<PlayerModel>();
                model.Damaged(damamge);

            }
        }
    }

    #endregion

    #region 일반 패턴 3 - 강공

    public void Pattern3Attack()
    {
        pattern3Effect.SetActive(true);

        Collider[] hits = Physics.OverlapSphere(transform.position, 15f);

        foreach(Collider col in hits)
        {
            if (col.CompareTag("Player"))
            {
                SInfoAttack damamge = new SInfoAttack(
                    this.gameObject,
                    player.gameObject,
                    Mathf.RoundToInt(Stat.totalDamage),
                    null
                    );

                PlayerModel model = col.GetComponentInChildren<PlayerModel>();
                model.Damaged(damamge);

            }
        }    
    }

    public void Pattern3EffectOff()
    {
        pattern3Effect.SetActive(false);
    }

    #endregion

    #region 일반 패턴 4 - 돌진 조건
    IEnumerator CheckRush()
    {
        while (true)
        {
            if (!canRush)
            {
                rushTimer += Time.deltaTime;
                if (rushTimer >= 20f)
                {
                    rushTimer = 0;
                    canRush = true;
                }
            }
            yield return null;
        }
    }
    #endregion

    private void OnDrawGizmos()
    {
        Vector3 centor = transform.position + transform.forward * 5 + transform.up * 5;
        Vector3 halfSize = new Vector3(5, 5, 5); // OverlapBox에 넣은 값(half extents)

        Gizmos.color = Color.red;

        // 위치 + 크기 + 회전을 적용한 박스를 그리기 위해
        Gizmos.matrix = Matrix4x4.TRS(
            centor,               // 박스 중심
            Quaternion.identity,  // 회전
            Vector3.one           // 스케일(필요 없으면 1,1,1)
        );

        // 실제 크기 = halfSize * 2
        Gizmos.DrawWireCube(Vector3.zero, halfSize * 2f);
    }

}
