using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : PoolableMono
{
    public EnemySO enemySO;
    protected EnemyStat Stat;

    protected NavMeshAgent agent;
    [HideInInspector]
    public NavMeshAgent Agent => agent;

    protected Rigidbody rb;   
    protected Animator anim;
    protected LineRenderer lr;

    [HideInInspector]
    public Animator Anim => anim;
    [HideInInspector]
    public LineRenderer Lr => lr;

    [HideInInspector]
    public float lastAttackTime;
    [HideInInspector]
    public Transform player;

    protected IAttackBehavior attackBehavior;
    [HideInInspector]
    public IAttackBehavior AttackBehavior => attackBehavior;

    protected StateMachine fsm;
    [HideInInspector]
    public StateMachine Fsm => fsm;

    public bool isFixedType = false;

    public Coroutine attackCoroutine;


    #region Unity Event
    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        player = GameObject.FindAnyObjectByType<PlayerController>().transform;
        fsm = new StateMachine();
        Reset();
    }

    protected virtual void Start()
    {
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.positionCount = 2;
        lr.enabled = false;
    }

    private void Update()
    {
        fsm.Tick();
    }

    private void FixedUpdate()
    {
        fsm.FixedTick();
    }

    protected virtual void OnEnable()
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

    public void StartAttackCoroutine(IEnumerator routine)
    {
        attackCoroutine = StartCoroutine(routine);
    }

    public virtual void StartAttack(int pattenrIndex = 0)
    {
        attackBehavior.ExecuteAttack(this);
    }

    public virtual IEnumerator AttackDelay(float delay)
    {
        Debug.Log("공격 딜레이 시작");
        yield return new WaitForSeconds(delay);
        fsm.ChangeState(new State_Chase(this, fsm));
        Debug.Log("공격 딜레이 종료");         
    }

    public virtual void TakeDamage(float amount)
    {
        Stat.curHp -= amount;
        if (Stat.curHp <= 0)
        {
            Die();
        }
    }
    
    protected virtual void Die()
    {
        if (Random.Range(0, 100f) <= enemySO.itemDropPersent)
        {
            player.gameObject.transform.GetChild(1).GetComponent<PlayerModel>().Stat.AddExp(enemySO.gainExp);
            FindAnyObjectByType<MainUIManager>().UpdateExp(player.gameObject.transform.GetChild(1).GetComponent<PlayerModel>());
            // 아이템 드랍
            TryDropItem(enemySO.itemDropTable);
            PoolManager.Instance.Push(this);
        }
        else
        {
            player.gameObject.transform.GetChild(1).GetComponent<PlayerModel>().Stat.AddExp(enemySO.gainExp);
            FindAnyObjectByType<MainUIManager>().UpdateExp(player.gameObject.transform.GetChild(1).GetComponent<PlayerModel>());
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
