using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : PoolableMono
{
    public EnemySO enemySO;
    private EnemyStat Stat;

    [HideInInspector]
    public NavMeshAgent agent;
    [HideInInspector]
    public Rigidbody rb;
    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public float lastAttackTime;
    [HideInInspector]
    public Transform player;

    public IAttackBehavior attackBehavior;

    private StateMachine fsm;


    #region Unity Event
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        fsm = new StateMachine();
    }

    private void Start()
    {
        //Invoke(fsm.ChangeState())
    }

    private void Update()
    {
        fsm.Tick();
    }

    private void FixedUpdate()
    {
        fsm.FixedTick();
    }

    private void OnEnable()
    {
        
    }
    #endregion

    public override void Reset()
    {
        Stat = new EnemyStat(enemySO);
    }

    public void StartAttack()
    {
        if (Time.time - lastAttackTime >= Stat.attackSpeed)
        {
            attackBehavior.ExecuteAttack(this);
            lastAttackTime = Time.time;
        }
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
