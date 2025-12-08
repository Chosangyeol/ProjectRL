using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3Boss : BossBase
{
    public GameObject Centor;
    public GameObject pattern2Projectile;
    public GameObject pattern2Warning;
    public Transform  pattenr3FirePos;
    public GameObject pattern3Projectile;

    public float special1Timer = 0f;
    public bool isSpecial1Playing = false;
    public float flyDuration = 2f;
    public float floodDuration = 2f;
    public GameObject Special1Object;

    public bool isSpecial2On = false;
    public Transform[] spawnPos;
    public GameObject pattern2Object;

    public bool isSpecial3 = false;

    public Renderer bossRightWing;
    public Renderer bossLeftWing;
    public Color special1WingColor;
    public Color special2WingColor;




    private void Start()
    {
        attackBehavior = new Stage3BossAttack(Centor, pattern2Projectile, pattern2Warning, pattenr3FirePos, pattern3Projectile);

        lr = GetComponent<LineRenderer>();
        lr.startWidth = 0.2f;
        lr.endWidth = 0.2f;
        lr.enabled = false;
        
    }

    private void Update()
    {
        if (!isSpecial1Playing)
            special1Timer += Time.deltaTime;
        if (special1Timer >= 10f)
            isSpecialPattern = true;
        CheckSpecial1();
        fsm.Tick();
    }

    public override void TakeDamage(int amount)
    {
        Stat.curHp -= amount;
        if (Stat.curHp < (Stat.totalHp * 0.5f) && !isSpecial2On)
        {
            isSpecial2On = true;
            StartCoroutine(SpecialPattern2());
            Debug.Log("보스 스페셜 패턴2 발동");
        }

        if (Stat.curHp < (Stat.totalHp * 0.3f) && !isSpecial3)
        {
            isSpecial3 = true;
            Debug.Log("보스 스페셜 패턴3 발동");
        }
    }

    public void CheckSpecial1()
    {
        if (!isSpecialPattern) return;
        if (isSpecial1Playing) return;
        if (!(attackBehavior is Stage3BossAttack bossAttack)) return;
        if (bossAttack.isAttacking) return;
        Debug.Log("보스 스페셜 패턴1 시작 가능");
        fsm.ChangeState(new State_BossSpecialPattern(this, fsm, patternCount));
        StartCoroutine(SpecialPattern1());

    }

    IEnumerator SpecialPattern1()
    {
        isSpecial1Playing = true;
        special1Timer = 0f;

        Vector3 startPos = Centor.transform.position;
        Vector3 endPos = startPos + Vector3.up * 10f;

        Vector3 dir = (endPos - startPos).normalized;

        Color defaultColor = bossRightWing.material.color;
        bossRightWing.material.color = special1WingColor;

        float t = 0f;

        while (t < flyDuration)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, t / flyDuration);
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        Vector3 floodOrigin = Special1Object.transform.position;
        Vector3 floodPos = Special1Object.transform.position + Vector3.up * 3f;
        Special1Object.GetComponent<PoisonEffect>().isActive = true;

        t = 0;
        while (t < floodDuration)
        {
            t += Time.deltaTime;
            Special1Object.transform.position = Vector3.Lerp(floodOrigin, floodPos, t / floodDuration);
            yield return null;
        }
        
        if (!isSpecial3)
        {
            yield return new WaitForSeconds(3f);
            t = 0;
            while (t < floodDuration)
            {
                t += Time.deltaTime;
                Special1Object.transform.position = Vector3.Lerp(floodPos, floodOrigin, t / floodDuration);
                yield return null;
                Special1Object.GetComponent<PoisonEffect>().isActive = false;
            }
        }
        
        Debug.Log("종료");

        if (!isSpecial3)
            bossRightWing.material.color = defaultColor;

        t = 0f;
        while (t < flyDuration)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(endPos, startPos, t / flyDuration);
            yield return null;
        }

        isSpecialPattern = false;

        if (!isSpecial3)
            isSpecial1Playing = false;

        StartCoroutine(AttackDelay(2f));
    }

    IEnumerator SpecialPattern2()
    {
        bossLeftWing.material.color = special2WingColor;
        while (true)
        {
            for (int i = 0; i < spawnPos.Length; i++)
            {
                PoolableMono obj = PoolManager.Instance.Pop(pattern2Object.name);
                obj.GetComponent<Pattern2Object>().SetTarget(player);
                obj.transform.position = spawnPos[i].position;
            }
            Debug.Log("벌레 소환");
            yield return new WaitForSeconds(5f);
        }
    }
}
