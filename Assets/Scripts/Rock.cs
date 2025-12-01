using System.Collections;
using UnityEngine;

namespace Rock
{
    public class Rock : MonoBehaviour
    {
        [Header("Fly Settings")]
        public Transform flyTarget;
        public float delayBeforeFly = 15.0f;
        public float flySpeed = 10f;
        public float lifetime = 10f;

        [Header("Spin Settings")]
        public float spinSpeed = 45f;

        private bool isFlying = false;

        private void OnEnable()
        {
            StartCoroutine(StartFlyingAfterDelay());
        }

        private IEnumerator StartFlyingAfterDelay()
        {
            yield return new WaitForSeconds(delayBeforeFly);

            isFlying = true;
            Destroy(gameObject, lifetime);
        }

        private void Update()
        {
            if (isFlying)
            {
                FlyTowardTarget();
            }
        }

        private void FlyTowardTarget()
        {
            if (flyTarget == null)
                return;

            Vector3 dir = flyTarget.position - transform.position;

            if (dir.sqrMagnitude < 0.001f)
            {
                Destroy(gameObject);
                return;
            }

            dir = dir.normalized;
            transform.position += dir * flySpeed * Time.deltaTime;
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = lookRot * Quaternion.Euler(0, 0, spinSpeed * Time.time);
        }
    }
}
