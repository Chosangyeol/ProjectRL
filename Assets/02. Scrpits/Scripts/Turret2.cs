using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Turret2 : InteractableObject, IInteractable
{
    [Header("Turret Settings")]
    public GameObject bulletPrefab;
    public Transform[] firePoints;
    public float fireRate = 2f;
    public float detectRange = 10f;
    public GameObject objectToDisable;
    public GameObject objectToDestroy;

    private PlayerModel plyr;

    [Header("Turret HP")]
    public int maxHP = 100;
    private int currentHP;

    private float fireTime = 0f;
    private Transform targetEnemy;
    private bool isActivated = false;
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
            Instantiate(bulletPrefab, fp.position, rot);

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


    public string interactName { get; }

    public void OnFocus()
    {
        interactCanvas.SetActive(true);
        interactCanvas.GetComponentInChildren<TMP_Text>().text = interactName + " - " + price + "G";
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

            if (objectToDestroy != null)
                Destroy(objectToDestroy);
        }
        else
        {
            Debug.Log("돈 부족");
        }
    }
}