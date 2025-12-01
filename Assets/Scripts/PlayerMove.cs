using UnityEngine;





    public class PlayerMove : MonoBehaviour
    {
        [Header("이동 속도 설정")]
        [Tooltip("플레이어 이동 속도 (m/s)")]
        public float moveSpeed = 5f;

        void Update()
        {
            // 입력값 받기 (WASD)
            float h = Input.GetAxis("Horizontal"); // A, D 키 (-1 ~ 1)
            float v = Input.GetAxis("Vertical");   // W, S 키 (-1 ~ 1)

            // 이동 방향 계산 (월드 기준)
            Vector3 moveDir = new Vector3(h, 0f, v);

            // 정규화해서 대각선 이동 속도 보정
            if (moveDir.magnitude > 1f)
                moveDir.Normalize();

            // 이동 적용
            transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);
        }
    }
