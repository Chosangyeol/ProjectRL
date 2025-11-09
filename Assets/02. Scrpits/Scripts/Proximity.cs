using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace proximity
{
    public class Proximity : MonoBehaviour
    {
        public float detectionRadius = 10f;
        public GameObject targetObject; // 껐다 켤 오브젝트
        private GameObject player;


        //드론이랑 터렛에 붙어있는 불 vfx 플레이어랑 거리가 있으면 안보이게 하는 스크립트
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");

        }

        void Update()
        {
            if (player == null || targetObject == null) return;

            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance > detectionRadius)
            {
                if (targetObject.activeSelf)
                    targetObject.SetActive(false);
            }
            else
            {
                if (!targetObject.activeSelf)
                    targetObject.SetActive(true);
            }
        }
    }
}
