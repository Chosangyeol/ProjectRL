using Info;
using Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Poison
{
    public class Posion : MonoBehaviour
    {
        public int dmg = 5;          // 총 데미지량

        public BuffType type;
        private bool damaging = false;
        private float prog = 0f;      // 진행도
        private PlayerModel player;


        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                player = other.GetComponentInChildren<PlayerModel>();
                if (player != null)
                {
                    damaging = true;
                    prog = 0f;
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                prog += Time.deltaTime;
                if (damaging && prog >= 1)
                {
                    SInfoBuff buff = new SInfoBuff(
                        this.gameObject,
                        other.gameObject,
                        new SpeedBuff(player, 30f, "이동속도 감소", type, 0.3f, false)
                        );

                    SInfoAttack attack = new SInfoAttack(
                        this.gameObject,
                        other.gameObject,
                        dmg,
                        null
                        );

                    PlayerModel model = other.GetComponentInChildren<PlayerModel>();

                    if (model.Buff.HasBuffByType(type))
                    {
                        model.Buff.UnactiveBuffByType(type);
                    }

                    model.AddBuff(buff);
                    model.Damaged(attack);

                    prog = 0f;
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                damaging = false;
                prog = 0f;
                player = null;
            }
        }
    }
}