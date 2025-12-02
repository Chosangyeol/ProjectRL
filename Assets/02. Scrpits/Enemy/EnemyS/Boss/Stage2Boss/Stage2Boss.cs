using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Boss : BossBase
{

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
    public Transform[] sequence2Targets;
    public Transform[] sequence3Targets;

    


    private void Start()
    {
        attackBehavior = new Stage2BossAttack();
    }

    private void Update()
    {
        PoisonPattern();
        SpecialPattern();
        fsm.Tick();
    }

    public override void TakeDamage(float amount)
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
        isPoisonPlaying = true;
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


    #endregion

}
