using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace chest
{
    public class Chest : MonoBehaviour
    {
        public List<GameObject> items; // 아이템 프리팹들
        public int price = 25;
        public float detectRange = 10f;

        private Transform plyr;
        private bool opened = false;   // 1회용 상자를 위한 변수

        void Update()
        {

            if (plyr == null)
            {
                GameObject p = GameObject.FindGameObjectWithTag("Player");
                if (p != null) plyr = p.transform;
            }

            // 사거리 안에서 5f(조절 가능)E 키를 눌렀을때 
            if (plyr != null && Vector3.Distance(transform.position, plyr.position) < detectRange)
            {
                if (!opened && Input.GetKeyDown(KeyCode.E))
                {
                    PlayerKM pScript = plyr.GetComponent<PlayerKM>();
                    if (pScript != null && pScript.money >= price)
                    {
                        pScript.money -= price;
                        string tag = RandTag();        // 확률로 태그 선택
                        SpawnItem(tag);                // 해당 태그 프리팹 소환
                        opened = true;                 // 한 번만 열림
                    }
                    else
                    {
                        Debug.Log("돈 부족");
                    }
                }
            }
        }

        // 확률 기반 태그 반환
        string RandTag()
        {
            float r = Random.Range(0f, 100f);
            if (r < 5f) return "Red"; //전설
            else if (r < 20f) return "Yellow";// 고급 이렇게?

            else if (r < 55f) return "Blue";
            else return "Green";
        }

        // 해당 태그를 가진 프리팹 중 랜덤 소환
        void SpawnItem(string tag)
        {
            List<GameObject> match = new List<GameObject>();

            foreach (GameObject obj in items)
            {
                if (obj != null)
                {
                    if (obj.CompareTag(tag))
                        match.Add(obj);
                }
            }

            if (match.Count > 0)
            {
                GameObject pick = match[Random.Range(0, match.Count)];
                GameObject go = Instantiate(pick, transform.position, Quaternion.identity);

                // 아이템 생동감
                Rigidbody rb = go.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 dir = Vector3.up + new Vector3(Random.Range(-0.2f, 0.2f), 0f, Random.Range(-0.2f, 0.2f));
                    rb.AddForce(dir.normalized * 5f, ForceMode.Impulse);
                }
            }
        }
    }
}
