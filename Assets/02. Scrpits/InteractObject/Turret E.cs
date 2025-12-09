using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TurretE : MonoBehaviour
{
    public float detectRange = 5f;           // 감지 사거리
    public GameObject targetObject;           // 활성/비활성화할 오브젝트
    private Transform player;

    //플레이어가 사거리 안에 감지 하면 터렛이랑 드론 오브젝트 위에 회전하는 오브젝트

    void Start()
    {
        if (targetObject == null)
            Debug.LogError("targetObject");

        
        targetObject.SetActive(false);

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
        else Debug.LogError("Player 태그");
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= detectRange)
            targetObject.SetActive(true);
        else
            targetObject.SetActive(false);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
