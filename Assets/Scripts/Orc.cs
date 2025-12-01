using System.Collections;
using UnityEngine;

namespace Orc
{
    public class Orc : MonoBehaviour
    {
        private Animator ani;
        private bool dead = false;

        public Transform target;
        public float atkRange = 10f; 
        public int hp = 100;
        private int maxHp;

        public float speed = 3.5f;

        public GameObject poison;
        public Transform mouth;
        private int lastPoisonTrigger = 0;

        private bool attacking = false;
        private bool isInCooldown = false;
        private bool preventMove = false;
        private float preventMoveTime = 0.2f;
        private float preventMoveTimer = 0f;

        public float atkCd = 2f;

        public GameObject leftAxe;
        public GameObject rightAxe;

        public float atkTime = 1f;
        public float atkRecover = 0.7f;

        public GameObject dangerPing;
        public GameObject dangerAttack;

        public GameObject dashHitbox;
        public float dashSpeed = 15f;
        public float dashTime = 1f;

        public bool phase2 = false;
        private bool phase2Active = false;
        public Transform centerPoint;
        public GameObject[] rocks;
        public GameObject[] warningZones;

        void Awake()
        {
            ani = GetComponent<Animator>();
            ani.SetTrigger("IdleTrigger");

            maxHp = hp;

            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) target = playerObj.transform;

            DisableAxes();

            if (dangerPing) dangerPing.SetActive(false);
            if (dangerAttack) dangerAttack.SetActive(false);
            if (dashHitbox) dashHitbox.SetActive(false);

            foreach (GameObject r in rocks)
                if (r != null) r.SetActive(false);

            foreach (GameObject w in warningZones)
                if (w != null) w.SetActive(false);
        }

        void Update()
        {
            if (dead) return;

            if (preventMove)
            {
                preventMoveTimer += Time.deltaTime;
                if (preventMoveTimer >= preventMoveTime)
                {
                    preventMove = false;
                    preventMoveTimer = 0f;
                }
                else
                {
                    ani.SetTrigger("IdleTrigger");
                    return;
                }
            }

         
            // P2 진입 체크
           
            if (!phase2 && hp <= maxHp * 0.5f)
            {
                phase2 = true;
                attacking = true;
                phase2Active = true;
                StartCoroutine(Phase2_Pattern());
            }

           
            // P1 독 공격 체력 트리거
          
            if (!phase2Active)
            {
                int poisonTrigger = (maxHp - hp) / (maxHp / 10);
                if (poisonTrigger > lastPoisonTrigger)
                {
                    lastPoisonTrigger = poisonTrigger;

                    StopAllCoroutines();
                    attacking = true;
                    isInCooldown = false;
                    StartCoroutine(Attack1_Poison());
                }
            }

           
            if (phase2Active) return;

           
            if (target != null)
            {
                Vector3 diff = transform.position - target.position;
                float distSqr = diff.sqrMagnitude;
                bool inRange = distSqr <= atkRange * atkRange;

                // 이동 처리
                if (!attacking && !isInCooldown)
                {
                    if (!inRange)
                    {
                        ani.SetTrigger("MoveTrigger");
                        MoveToTarget();
                    }
                    else
                    {
                        ani.SetTrigger("IdleTrigger");
                    }
                }
                else
                {
                    ani.SetTrigger("IdleTrigger");
                }

                // 공격 처리
                if (!attacking && !isInCooldown && inRange)
                {
                    PerformAttack();
                }
            }
        }

        private void MoveToTarget()
        {
            if (target == null) return;
            Vector3 dir = (target.position - transform.position).normalized;
            dir.y = 0;
            transform.position += dir * speed * Time.deltaTime;
            transform.LookAt(target);
        }

        private void ResetState()
        {
            hp = maxHp;
            attacking = false;
            isInCooldown = false;
            preventMove = false;
            preventMoveTimer = 0f;
            phase2Active = false;
            StopAllCoroutines();
            ani.SetTrigger("IdleTrigger");
            lastPoisonTrigger = 0;
            DisableAxes();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Attack") && !dead)
            {
                hp -= 5;
                if (hp <= 0) Die();
            }
        }

        private void Die()
        {
            dead = true;
            ani.SetTrigger("DeathTrigger");
            StopAllCoroutines();
            DisableAxes();
        }

        
        // 랜덤 공격 선택
       
        public void PerformAttack()
        {
            if (attacking || dead) return;

            attacking = true;

            int choice = Random.Range(0, 4);

            if (choice == 0) StartCoroutine(Attack2_Axe());
            else if (choice == 1) StartCoroutine(Attack3_Spin());
            else if (choice == 2) StartCoroutine(Attack4_Danger());
            else if (choice == 3) StartCoroutine(Attack5_Dash());
        }

        private IEnumerator Cooldown()
        {
            isInCooldown = true;
            yield return new WaitForSeconds(atkCd);
            attacking = false;
            isInCooldown = false;

            preventMove = true;
            preventMoveTimer = 0f;

            ani.SetTrigger("IdleTrigger");
        }

       
        // 공격 패턴들
        

        private IEnumerator Attack1_Poison()
        {
            ani.SetTrigger("Attack1Trigger");
            yield return new WaitForSeconds(1.5f);

            if (poison != null && mouth != null && target != null)
            {
                Vector3 dir = (target.position - mouth.position).normalized;
                Instantiate(poison, mouth.position, Quaternion.LookRotation(dir));
            }

            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Cooldown());
        }

        private IEnumerator Attack2_Axe()
        {
            ani.SetTrigger("Attack2Trigger");
            EnableAxes();
            yield return new WaitForSeconds(atkTime);
            DisableAxes();
            yield return new WaitForSeconds(atkRecover);
            StartCoroutine(Cooldown());
        }

        private IEnumerator Attack3_Spin()
        {
            ani.SetTrigger("Attack3Trigger");
            EnableAxes();
            yield return new WaitForSeconds(atkTime);
            DisableAxes();
            yield return new WaitForSeconds(atkRecover);
            StartCoroutine(Cooldown());
        }

        private IEnumerator Attack4_Danger()
        {
            ani.SetTrigger("Attack4Trigger");
            if (dangerPing) dangerPing.SetActive(true);
            yield return new WaitForSeconds(3f);
            if (dangerAttack) dangerAttack.SetActive(true);
            if (dangerPing) dangerPing.SetActive(false);
            yield return new WaitForSeconds(1f);
            if (dangerAttack) dangerAttack.SetActive(false);
            StartCoroutine(Cooldown());
        }

        private IEnumerator Attack5_Dash()
        {
            ani.SetTrigger("Attack5Trigger");
            if (dashHitbox) dashHitbox.SetActive(true);

            Vector3 dashDir = target.position - transform.position;
            dashDir.y = 0;
            dashDir.Normalize();

            if (dashDir != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(dashDir);

            float timer = 0f;
            while (timer < dashTime)
            {
                transform.position += dashDir * dashSpeed * Time.deltaTime;
                timer += Time.deltaTime;
                yield return null;
            }

            if (dashHitbox) dashHitbox.SetActive(false);
            StartCoroutine(Cooldown());
        }

       
        // Phase2 패턴
    
        private IEnumerator Phase2_Pattern()
        {
            ani.SetTrigger("JumpTrigger");

            float jumpTime = 0.5f;
            float timer = 0f;

            // 위로 뛰어오름
            Vector3 startPos = transform.position;
            Vector3 upPos = startPos + Vector3.up * 10f;

            while (timer < jumpTime)
            {
                transform.position = Vector3.Lerp(startPos, upPos, timer / jumpTime);
                timer += Time.deltaTime;
                yield return null;
            }

            ani.SetTrigger("RoarTrigger");
            yield return new WaitForSeconds(0.5f);

            if (centerPoint != null)
                transform.position = centerPoint.position + Vector3.up * 7f;

            yield return new WaitForSeconds(0.2f);

            // 중앙 착지
            timer = 0f;
            Vector3 endPos = centerPoint.position;

            while (timer < jumpTime)
            {
                transform.position = Vector3.Lerp(transform.position, endPos, timer / jumpTime);
                timer += Time.deltaTime;
                yield return null;
            }

            // 바위 생성
            foreach (GameObject r in rocks)
                if (r != null) r.SetActive(true);

            // 경고 패턴 순차 실행
            yield return new WaitForSeconds(3f);
            ActivateWarning(0, 2);
            yield return new WaitForSeconds(3f);
            ActivateWarning(2, 2);
            yield return new WaitForSeconds(3f);
            ActivateWarning(4, 6);
            yield return new WaitForSeconds(9f);

            attacking = false;
            phase2Active = false;
        }

        private void ActivateWarning(int startIndex, int count)
        {
            for (int i = startIndex; i < startIndex + count; i++)
            {
                if (i < warningZones.Length && warningZones[i] != null)
                {
                    warningZones[i].SetActive(true);
                    StartCoroutine(DisableWarningAfterDelay(warningZones[i]));
                }
            }
        }

        private IEnumerator DisableWarningAfterDelay(GameObject warning)
        {
            yield return new WaitForSeconds(2f);
            if (warning != null)
                warning.SetActive(false);
        }

        private void EnableHit(GameObject obj)
        {
            if (obj == null) return;
            var col = obj.GetComponent<BoxCollider>();
            if (col != null) col.enabled = true;
        }

        private void DisableHit(GameObject obj)
        {
            if (obj == null) return;
            var col = obj.GetComponent<BoxCollider>();
            if (col != null) col.enabled = false;
        }

        private void EnableAxes()
        {
            EnableHit(leftAxe);
            EnableHit(rightAxe);
        }

        private void DisableAxes()
        {
            DisableHit(leftAxe);
            DisableHit(rightAxe);
        }
    }
}
