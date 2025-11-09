using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hpmoney
{
    public class HPMoney : MonoBehaviour
    {


        public float detectRange = 10f;
        private Transform player;
        private bool isActivated = false;

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
                        if (playerScript != null && playerScript.HP >= 25)
                        {
                            playerScript.HP -= 25; // 돈 차감
                            playerScript.money += 25; // 피 증가
                            isActivated = true;
                        }
                        else
                        {
                            Debug.Log("피 부족.");
                        }
                    }
                }


            }

        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
            Gizmos.DrawWireSphere(transform.position, detectRange);
        }


    }
}