using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemySO", menuName = "EnemySO")]
public class EnemySO : ScriptableObject
{
    public string enemyName;

    [Header("���� ����")]
    public int cost;
    public int weight = 1;
    public int spawnCount;

    [Header("���� ���� ���� (����)")]
    public float baseHp;
    public float baseDamage;

    [Header("���� ���� ���� (����)")]
    public float attackRange;
    public float patrolRange;
    public float detectRange;
    public float moveSpeed;
    public float attackSpeed;

}
