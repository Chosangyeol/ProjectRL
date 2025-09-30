using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : PoolableMono
{
    public EnemySO enemySO;
    private EnemyStat Stat;
    

    [HideInInspector]
    public Rigidbody rb;
    [HideInInspector]
    public Animator anim;


    #region Unity Event
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        
    }
    #endregion

    public override void Reset()
    {
        Stat.EnemyReset(enemySO);
    }

    public void TakeDamage(float amount)
    {
        Stat.curHp -= amount;
        if (Stat.curHp <= 0)
        {
            Die();
        }
    }
    
    public void Die()
    {
        PoolManager.Instance.Push(this);
    }
}
