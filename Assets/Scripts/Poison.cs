using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Poison
{
    public class Posion : MonoBehaviour
    {
        public int dmg = 25;          // 총 데미지량
        public float dur = 5f;        // 데미지를 주는 시간
        private float rate;           // 초당 데미지량

        private bool damaging = false;
        private float prog = 0f;      // 진행도
        private int done = 0;         // 누적 데미지량
        private PlayerModel player;

        void Start()
        {
            rate = dmg / dur;         // 초당 데미지 계산
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                player = other.GetComponent<PlayerModel>();
                if (player != null)
                {
                    damaging = true;
                    done = 0;
                    prog = 0f;
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                damaging = false;
                player = null;
            }
        }

        //void Update()
        //{
        //    // 플레이어가 있고, 아직 데미지를 줄 양이 남아 있다면
        //    if (damaging && player != null && done < dmg)
        //    {
        //        prog += rate * Time.deltaTime;

        //        int damageNow = Mathf.FloorToInt(prog);
        //        if (damageNow > 0)
        //        {
        //            prog -= damageNow;
        //            int left = dmg - done;
        //            int apply = Mathf.Min(damageNow, left);

        //            player.HP -= apply;
        //            done += apply;

        //            // 체력이 0 이하로 떨어지지 않도록
        //            if (player.HP <= 0)
        //            {
        //                player.HP = 0;
        //                damaging = false;
        //            }
        //        }
        //    }
        //}
    }
}