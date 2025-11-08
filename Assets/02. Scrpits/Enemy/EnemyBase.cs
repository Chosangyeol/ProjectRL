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

    }

    private void Update()
    {
        fsm.Tick();
    }

    private void FixedUpdate()
    {
        fsm.FixedTick();
    }

    public virtual void OnEnable()
    {
        fsm.ChangeState(new State_Patrol(this, fsm));
    }
    #endregion

    public override void Reset()
    {
        Stat = new EnemyStat(enemySO);
    }

    public EnemyStat GetStat()
    {
        return Stat;
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
        if (Random.Range(0, 100f) <= enemySO.itemDropPersent)
        {
            // 아이템 드랍
            TryDropItem(enemySO.itemDropTable);
            PoolManager.Instance.Push(this);
        }
        else
        {
            PoolManager.Instance.Push(this);
        }
    }

    // 아이템 드랍
    public void TryDropItem(DropTableSO table)
    {
        if (table == null) return;

        // 드랍될 아이템의 등급 정하기
        float groupResult = Random.Range(0f, 100f);
        float groupWeight = 0f;

        DropTableSO.RarityGroup selectedGroup = null;

        foreach (var group in table.rarityGroup)
        {
            groupWeight += group.rarityWeight;
            if (groupResult <= groupWeight)
            {
                selectedGroup = group;
                break;
            }
        }

        if (selectedGroup == null || selectedGroup.items.Count == 0) return;

        // 정해진 등급 안에서 아이템 드랍하기
        float itemResult = Random.Range(0f, 100f);
        float itemWeight = 0f;

        PoolableMono selectedItem = null;

        foreach (var item in selectedGroup.items)
        {
            itemWeight += item.weight;
            if (itemResult <= itemWeight)
            {
                selectedItem = item.item;
                break;
            }
        }

        if (selectedItem != null)
        {
            PoolableMono dropItem = PoolManager.Instance.Pop(selectedItem.name);
            dropItem.gameObject.transform.position = this.gameObject.transform.position;
        }
    }
}
