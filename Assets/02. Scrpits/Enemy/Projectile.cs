using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : PoolableMono
{
    public EnemyBase owner;
    [HideInInspector]
    public float damage;
    [HideInInspector]
    public float timer;
    public float destroyTime;
    public float speed;

    public bool isParabolic = false;

    private Vector3 startPos;
    private Vector3 targetPos;
    private Vector3 peakPos;
    private float flightTime;
    private float currentTime; 


    public override void Reset()
    {
        base.Reset();
        currentTime = 0f;
        timer = Time.time;
    }

    private void Update()
    {
        if (isParabolic)
        {
            ParabolicMove();
        }

        if (Time.time - timer >= destroyTime)
        {
            PoolManager.Instance.Push(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 적중");
            PoolManager.Instance.Push(this);
        }
    }

    public void SetParabolicMove(Vector3 start, Vector3 target, float heihgt, float time)
    {
        isParabolic = true;

        startPos = start;
        targetPos = target;
        flightTime = time;
        
        peakPos = (startPos + targetPos) / 2 + Vector3.up * heihgt;
    }

    private void ParabolicMove()
    {
        currentTime += Time.deltaTime;
        float t = Mathf.Clamp01(currentTime / flightTime);

        t = EaseInOutQuad(t);

        Vector3 pos = 
            (1-t) * (1-t) * startPos +
            2 * (1-t) * t * peakPos +
            t * t * targetPos;
        
        transform.position = pos;
    }

    float EaseInOutQuad(float x)
    {
        return x < 0.5f ? 2f * x * x : 1 - Mathf.Pow(-2 * x + 2, 2) * 0.5f;
    }
}
