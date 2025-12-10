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

        int difficultyIndex = GameManager.Instance.Difficulty;

        float difficultyMultiplier = difficultyIndex == 0 ? 0.75f : 1.0f;
        // timer(초)에 따라 난이도 배율 계산
        float difficultyGrowth = Mathf.Pow(growthPerSecond, GameManager.Instance.Timer * difficultyMultiplier);

        totalHp = Mathf.RoundToInt(baseHp * difficultyGrowth);
        totalDamage = Mathf.RoundToInt(baseDamage * difficultyGrowth);
    }
}
