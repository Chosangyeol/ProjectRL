using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemySO", menuName = "EnemySO")]
public class EnemySO : ScriptableObject
{
    public string enemyName;

    [Header("스폰 세팅")]
    public int[] weight;
    public int spawnCount;

    [Header("몬스터 스텟 세팅 (성장)")]
    public int baseHp;
    public int baseDamage;

    [Header("몬스터 스텟 세팅 (고정)")]
    public float attackRange;
    public float patrolRange;
    public float detectRange;
    public float detectAngle = 90f;
    public float moveSpeed;
    public float attackSpeed;
    public int   gainExp;

    [Header("아이템 드랍 테이블")]
    public DropTableSO itemDropTable;
    [Range(0f, 100f)]
    public float itemDropPersent;

}
