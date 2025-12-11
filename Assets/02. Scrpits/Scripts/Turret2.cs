using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Turret2 : InteractableObject, IInteractable
{
    [Header("Turret Settings")]
    public PoolableMono bulletPrefab;
    public Transform[] firePoints;
    public float fireRate = 2f;
    public float detectRange = 10f;
    public int turretDamage;
    public GameObject objectToDisable;
    public GameObject objectToDestroy;

    private PlayerModel plyr;

    [Header("Turret HP")]
    public int maxHP = 100;
    private int currentHP;

    private float fireTime = 0f;
    private Transform targetEnemy;
    private bool isActivated = false;
    public bool IsActivated => isActivated;
    private int currentFireIndex = 0;
    private Animator anim;
    private AudioSource audioSource;
    private float detectRangeSqr;

    private Func<int> getMoneyFunc;
    private Action<int> spendMoneyFunc;

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null) audioSource.enabled = false;
        plyr = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerModel>();
        detectRangeSqr = detectRange * detectRange;
        currentHP = maxHP;
    }

    void Update()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, detectRange, LayerMask.GetMask("Enemy"));
        Transform closestEnemy = null;
        float minDist = Mathf.Infinity;

        foreach (Collider e in cols)
        {
            float dist = (transform.position - e.transform.position).sqrMagnitude;
            if (dist < minDist)
            {
                minDist = dist;
                closestEnemy = e.transform;
            }
        }

        targetEnemy = closestEnemy;

        if (targetEnemy == null && getMoneyFunc == null) return;

        float distSqr = targetEnemy != null ? (transform.position - targetEnemy.position).sqrMagnitude : 0f;

        if (isActivated && targetEnemy != null && distSqr <= detectRangeSqr)
        {
            Vector3 dir = (targetEnemy.position - transform.position).normalized;
            dir.y = 0f;
            transform.rotation = Quaternion.LookRotation(dir);

            if (Time.time >= fireTime)
            {
                Shoot(targetEnemy);
                fireTime = Time.time + 1f / fireRate;
            }

            if (audioSource != null && !audioSource.enabled)
                audioSource.enabled = true;
        }
        else
        {
            if (audioSource != null && audioSource.enabled)
                audioSource.enabled = false;
        }

        if (Vector3.Distance(transform.position, plyr.transform.position) <= interactRange && !isActivated)
        {
            OnFocus();
        }
        else
        {
            OnUnFocus();
        }
    }

    void Shoot(Transform target)
    {
        if (bulletPrefab != null && firePoints.Length > 0 && target != null)
        {
            Transform fp = firePoints[currentFireIndex];
            Vector3 dir = (target.position - fp.position).normalized;
            Quaternion rot = Quaternion.LookRotation(dir);

            PoolableMono bullet = PoolManager.Instance.Pop(bulletPrefab.name);
            Projectile proj = bullet.GetComponent<Projectile>();
            bullet.transform.position = fp.position;
            bullet.transform.LookAt(firePoints[currentFireIndex].position + dir);
            proj.damage = turretDamage;
            proj.owner2 = this;

            

            proj.GetComponent<Rigidbody>().velocity =
                (targetEnemy.GetComponent<Collider>().bounds.center - fp.position).normalized * proj.GetComponent<Projectile>().speed;

            if (anim != null)
                anim.SetTrigger("Shoot");

            currentFireIndex = (currentFireIndex + 1) % firePoints.Length;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isActivated = false;

        if (anim != null)
            anim.enabled = false;

        if (audioSource != null)
            audioSource.enabled = false;

        if (objectToDisable != null)
            objectToDisable.SetActive(true);

        Destroy(gameObject, 1.5f);
    }

    [SerializeField]
    private string _interactName;
    public string interactName => _interactName;

    public void OnFocus()
    {
        interactCanvas.SetActive(true);
        interactCanvas.GetComponentInChildren<TMP_Text>().text = interactName + " - " + price + "G";
        Transform player = GameObject.FindAnyObjectByType<PlayerModel>().transform;

        // 플레이어 방향 벡터 계산
        Vector3 dir = player.position - interactCanvas.transform.position;
        dir.y = 0;  // 위아래 각도 제거

        // 방향이 0 벡터가 되지 않도록 체크
        if (dir.sqrMagnitude > 0.0001f)
        {
            interactCanvas.transform.rotation = Quaternion.LookRotation(dir);
        }

    }
    public void OnUnFocus()
    {
        interactCanvas.SetActive(false);
        interactCanvas.GetComponentInChildren<TMP_Text>().text = " ";
    }
    public void OnInteract()
    {
        int nowMoney = GameManager.Instance.Money;

        if (nowMoney >= price)
        {
            GameManager.Instance.RemoveMoney(price);
            isActivated = true;                 // 한 번만 열림

            if (objectToDisable != null)
                objectToDisable.SetActive(false);
        }
        else
        {
            Debug.Log("돈 부족");
        }
    }
}