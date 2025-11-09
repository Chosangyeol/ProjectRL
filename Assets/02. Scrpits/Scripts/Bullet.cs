using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bullet
{
    public class Bullet : MonoBehaviour
    {
        public float spd = 20f;     // 총알 속도
        public float life = 3f;     // 총알 시간

        private Rigidbody rb;

        void Awake()
        {

            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody>();
                rb.useGravity = false;
            }
        }

        void Start()
        {
            // 총알 정면으로 발사
            rb.velocity = transform.forward * spd;


            Destroy(gameObject, life);
        }

        void OnTriggerEnter(Collider other)
        {
            //태그를 활용한 플레이어 접촉시 파괴
            if (other.CompareTag("Player"))
            {
                Destroy(gameObject);
            }
            // 벽 태그에 접촉시 파괴
            // 적으로 태그를 바꾸면 사용 가능
            else if (other.CompareTag("Wall"))
            {
                Destroy(gameObject);
            }
        }
    }
}
