using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemySO", menuName = "EnemySO")]
public class EnemySO : ScriptableObject
{
    public string enemyName;

    [Header("스폰 세팅")]
    public int cost;
    public int weight = 1;
    public int spawnCount;

    [Header("몬스터 스텟 세팅 (성장)")]
    public float baseHp;
    public float baseDamage;

    [Header("몬스터 스텟 세팅 (고정)")]
    public float attackRange;
    public float patrolRange;
    public float detectRange;
    public float moveSpeed;
    public float attackSpeed;

}
