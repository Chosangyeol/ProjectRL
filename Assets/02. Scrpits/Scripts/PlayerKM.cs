using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKM : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int money = 100;
    public int HP = 100;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(h, 0f, v).normalized;
        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);

        if (moveDir != Vector3.zero)
        {
            rb.rotation = Quaternion.LookRotation(moveDir);
        }
    }

    // 트리거 접촉 로그
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("트리거 접촉: " + other.name + " / 태그: " + other.tag);
    }

    // 일반 충돌 로그
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("물리 충돌: " + collision.gameObject.name + " / 태그: " + collision.gameObject.tag);
    }
}