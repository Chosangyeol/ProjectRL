using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat
{
    public EnemySO enemySO;
    public int baseHp;
    public int totalHp;
    public int curHp;

    public int baseDamage;
    public int totalDamage;

    public float moveSpeed;
    public float attackSpeed;

    public EnemyStat(EnemySO enemySO)
    {
        EnemyReset(enemySO);
    }

    public void EnemyReset(EnemySO enemySO)
    {
        this.enemySO = enemySO;
        this.baseHp = enemySO.baseHp;
        this.baseDamage = enemySO.baseDamage;
        EnemyUpgrade();
        this.curHp = totalHp;
        this.moveSpeed = enemySO.moveSpeed;
        this.attackSpeed = enemySO.attackSpeed;
    }

    public void EnemyUpgrade()
    {
        float growthPerSecond = 1.001155f;

        // timer(초)에 따라 난이도 배율 계산
        float difficulty = Mathf.Pow(growthPerSecond, GameManager.Instance.Timer);

        totalHp = Mathf.RoundToInt(baseHp * difficulty);
        totalDamage = Mathf.RoundToInt(baseDamage * difficulty);
    }
}
