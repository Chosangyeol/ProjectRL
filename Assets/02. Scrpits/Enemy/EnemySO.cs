using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Common,
    Elite,
    Boss
}

[System.Serializable]
public struct EnemyBaseStats
{
    public float baseHp;
    public float baseDamage;
    public float moveSpeed;
    public float attackSpeed;
}

[CreateAssetMenu(menuName ="Game/EnemySO")]
public class EnemySO : ScriptableObject
{
    public string enemyId;
    public EnemyType enemyType;
    public GameObject prefab;

    public EnemyBaseStats stats;

    public float detectRange;
    public float attackRange;

    // 시간에 따른 적 스텟 상승 계수

    // 스테이지에 따른 적 스텟 상승 계수

}


