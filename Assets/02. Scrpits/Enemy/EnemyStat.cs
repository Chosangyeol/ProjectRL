using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat
{
    public EnemySO enemySO;
    public float baseHp;
    public float totalHp;
    public float curHp;

    public float baseDamage;
    public float totalDamage;

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
        // ���̵� ������ �ð� ������ ����Ͽ� ���� ���� ������ ����
        totalHp = baseHp * 1.0f;
        totalDamage = baseDamage * 1.0f;
    }
}
