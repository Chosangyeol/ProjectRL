using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace heal
{
    public class Heal : MonoBehaviour
    {
        public int amt = 25;          // 회복하는 량
        public float dur = 5f;        // 회복하는데 걸리는 시간
        private float rate;           // 초당 회복량

        private bool healing = false;
        private float prog = 0f;      // 진행도
        private int done = 0;         // 누적 회복량
        private Player player;

        void Start()
        {
            rate = amt / dur;         // 초당 회복량 계산
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                player = other.GetComponent<Player>();
                if (player != null)
                {
                    healing = true;
                    done = 0;
                    prog = 0f;
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                healing = false;
                player = null;
            }
        }

        void Update()
        {
            // 플레이어가 있고, 아직 회복할 양이 남아 있다면
            if (healing && player != null && done < amt)
            {
                prog += rate * Time.deltaTime;  // 진행도 증가

                int healNow = Mathf.FloorToInt(prog);  // 정수 단위 회복
                if (healNow > 0)
                {
                    prog -= healNow;  // 소수점 잔여 유지
                    int left = amt - done;
                    int apply = Mathf.Min(healNow, left);  // 남은 양만큼만 회복

                    player.HP += apply;
                    done += apply;

                    // 최대 체력 초과 방지
                    if (player.HP > 100)
                    {
                        player.HP = 100;
                        healing = false;
                    }
                }
            }
        }
    }
}
