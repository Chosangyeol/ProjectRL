using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hpHeal
{
    public class HPHeal : MonoBehaviour
    {
        public float detectRange = 10f;
        private Transform player;
        private bool isActivated = false;

        public GameObject targetObject; // 활성화할 오브젝트
        public int cost = 25; // 사용할 돈
        public float activeDuration = 4f; // 오브젝트 유지 시간(힐 장판)

        void Start()
        {
            // 시작할 때 오브젝트를 비활성화
            if (targetObject != null)
            {
                targetObject.SetActive(false);
            }
        }

        void Update()
        {
            if (player == null)
            {
                GameObject p = GameObject.FindGameObjectWithTag("Player");
                if (p != null) player = p.transform;
            }

            if (player != null)
            {
                float dist = Vector3.Distance(transform.position, player.position);

                if (!isActivated && dist <= detectRange)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        PlayerKM playerScript = player.GetComponent<PlayerKM>();
                        if (playerScript != null && playerScript.money >= cost)
                        {
                            // 돈 차감
                            playerScript.money -= cost;

                            // 오브젝트 활성화
                            if (targetObject != null)
                            {
                                targetObject.SetActive(true);
                                StartCoroutine(DeactivateAfterSeconds(activeDuration));
                            }

                            isActivated = true;
                        }
                        else
                        {
                            Debug.Log("돈 부족");
                        }
                    }
                }
            }
        }

        IEnumerator DeactivateAfterSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            if (targetObject != null)
            {
                targetObject.SetActive(false);
            }
            isActivated = false;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
            Gizmos.DrawWireSphere(transform.position, detectRange);
        }
    }
}
