using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

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

    public bool isFly = false;
    public float flyHeight = 0f;
    public float hover = 0.5f;
    
    public bool isDie = false;
    public bool IsDie => isDie;

    protected AudioSource audioS;

    [HideInInspector]
    public AudioSource AudioS => audioS;

    protected bool isAttacked = false;

    [Header("사운드")]
    public AudioClip attackClip;
    public AudioClip deathClip;

    #region Unity Event
    protected virtual void Awake()
    {
        if (!isFly)
            agent = GetComponent<NavMeshAgent>();

        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        lr = GetComponent<LineRenderer>();
        player = GameObject.FindAnyObjectByType<PlayerController>().transform;
        fsm = new StateMachine();
        audioS = GetComponent<AudioSource>();

        Reset();
    }

    protected virtual void Start()
    {
        if (lr != null)
        {
            lr.startWidth = 0.1f;
            lr.endWidth = 0.1f;
            lr.positionCount = 2;
            lr.enabled = false;
        }    
    }

    protected virtual void Update()
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
        if (agent != null)
        {
            agent.speed = Stat.moveSpeed;
            Debug.Log("이동속도 세팅");
        }
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
        anim.SetTrigger("Idle");
        yield return new WaitForSeconds(delay);
        if (isFly)
            fsm.ChangeState(new State_FlyChase(this, fsm));
        else
            fsm.ChangeState(new State_Chase(this, fsm));
        Debug.Log("공격 딜레이 종료");         
    }

    public void PlaySound(AudioClip clip)
    {
        audioS.Stop();
        audioS.clip = clip;
        audioS.Play();
    }

    public virtual void TakeDamage(int amount)
    {
        Stat.curHp -= amount;

        if (!isAttacked)
        {
            isAttacked = true;
            if (isFly)
                fsm.ChangeState(new State_FlyChase(this, fsm));
            else
                fsm.ChangeState(new State_Chase(this, fsm));
        }

        if (Stat.curHp <= 0 && !isDie)
        {
            isDie = true;
            StopAllCoroutines();
            PlaySound(deathClip);
            EnemyDirector ed = GameObject.FindAnyObjectByType<EnemyDirector>();
            if (ed != null)
                ed.IncreKillCount();

            fsm.ChangeState(new State_Die(this, fsm));

            PlayerModel model = player.GetComponentInChildren<PlayerModel>();

            if (Random.Range(0, 100f) <= enemySO.itemDropPersent)
            {
                model.Stat.AddExp(enemySO.gainExp);
                FindAnyObjectByType<MainUIManager>().UpdateExp(model);
                TryDropItem(enemySO.itemDropTable);
            }
            else
            {
                model.Stat.AddExp(enemySO.gainExp);
                FindAnyObjectByType<MainUIManager>().UpdateExp(model);
            }      
            Anim.SetTrigger("Die");
        }
    }
    
    protected virtual void Die()
    {
        PoolManager.Instance.Push(this);
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
            if (!isFly)
            {
                dropItem.gameObject.transform.position = this.gameObject.transform.position + new Vector3(0, 1f, 0);
            }
            else if (isFly)
            {
                RaycastHit hit;
                if (Physics.Raycast(this.transform.position,Vector3.down, out hit, 100f,LayerMask.GetMask("Ground")))
                {
                    dropItem.gameObject.transform.position = hit.point + new Vector3(0, 1f, 0);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (enemySO == null) return;

        Gizmos.color = Color.yellow;

        float detectRange = enemySO.detectRange;
        float fov = enemySO.detectAngle;

        Vector3 pos = transform.position;
        Vector3 forward = transform.forward;

        // ���� ��� ����
        Vector3 leftDir = Quaternion.Euler(0, -fov * 0.5f, 0) * forward;
        // ������ ��� ����
        Vector3 rightDir = Quaternion.Euler(0, fov * 0.5f, 0) * forward;

        // ��輱 �׸���
        Gizmos.DrawLine(pos, pos + leftDir * detectRange);
        Gizmos.DrawLine(pos, pos + rightDir * detectRange);

        // ��ȣ(Arc) �׸���
        int segments = 30;
        float deltaAngle = fov / segments;
        Vector3 prevPoint = pos + leftDir * detectRange;

        for (int i = 1; i <= segments; i++)
        {
            Vector3 nextDir = Quaternion.Euler(0, -fov * 0.5f + deltaAngle * i, 0) * forward;
            Vector3 nextPoint = pos + nextDir * detectRange;
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }
}
