using Player;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Stage1Range : EnemyBase
{
    public GameObject projectile;
    public Transform firePos;
    public float attackPreDelay;
    public GameObject deathEffect;

    protected override void Awake()
    {
        base.Awake();
        lr = GetComponent<LineRenderer>();
        deathEffect.SetActive(false);

    }

    protected override void Start()
    {
        base.Start();

        attackBehavior = new RangedAttack(projectile, firePos, attackPreDelay);
    }

    public override void TakeDamage(int amount)
    {
        Stat.curHp -= amount;

        if (!isAttacked)
        {
            isAttacked = true;
            if (isFly)
                fsm.ChangeState(new State_FlyChase(this, fsm));
            else
                fsm.ChangeState(new State_Chase(this, fsm));
        }

        if (Stat.curHp <= 0 && !IsDie)
        {
            isDie = true;
            StopAllCoroutines();

            EnemyDirector ed = GameObject.FindAnyObjectByType<EnemyDirector>();
            if (ed != null)
                ed.IncreKillCount();

            fsm.ChangeState(new State_Die(this, fsm));

            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        deathEffect.SetActive(true);
        audioS.clip = deathClip;
        audioS.Play();
        yield return new WaitForSeconds(1f);

        PlayerModel model = player.GetComponentInChildren<PlayerModel>();
        int dropMoney = Random.Range(enemySO.minDropMoney, enemySO.maxDropMoney);

        if (Random.Range(0, 100f) <= enemySO.itemDropPersent)
        {
            model.Stat.AddExp(enemySO.gainExp);
            GameManager.Instance.AddMoney(dropMoney);
            FindAnyObjectByType<MainUIManager>().UpdateExp(model);
            TryDropItem(enemySO.itemDropTable);
        }
        else
        {
            model.Stat.AddExp(enemySO.gainExp);
            GameManager.Instance.AddMoney(dropMoney);
            FindAnyObjectByType<MainUIManager>().UpdateExp(model);
        }

        PoolManager.Instance.Push(this);
    }

}
