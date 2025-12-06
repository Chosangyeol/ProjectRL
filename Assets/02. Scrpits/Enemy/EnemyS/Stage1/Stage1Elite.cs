using Info;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Elite : EnemyBase
{
    public bool canRange = true;
    private float rangeTimer = 0;
    public float rangeCool = 10f;

    public PoolableMono projectile;
    public Transform firePos1;
    public Transform firePos2;

    protected override void Awake()
    {
        base.Awake();
        lr = GetComponent<LineRenderer>();

    }
    protected override void Start()
    {
        base.Start();

        attackBehavior = new Stage1EliteAttack(projectile, firePos1, firePos2);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(CheckRangeAttack());
    }

    protected void OnDisable()
    {
        StopCoroutine(CheckRangeAttack());
    }

    public void Attack()
    {
        Vector3 size = new Vector3(2f, 4f, 2f);
        Vector3 centor = transform.position + transform.forward * 2f;
        Collider[] hit = Physics.OverlapBox(centor, size);

        foreach (Collider col in hit)
        {
            PlayerModel other = col.GetComponentInChildren<PlayerModel>();

            if (other != null)
            {
                SInfoAttack attack = new SInfoAttack(
                    this.gameObject,
                    other.gameObject,
                    Mathf.RoundToInt(Stat.totalDamage),
                    null
                    );

                audioS.clip = attackClip;
                audioS.Play();
                other.Damaged(attack);
            }
        }
    }

    IEnumerator CheckRangeAttack()
    {
        while (true)
        {
            if (!canRange)
            {
                rangeTimer += Time.deltaTime;
                if (rangeTimer >= rangeCool)
                {
                    rangeTimer = 0;
                }
            }
            yield return null;
        }
    }

}
