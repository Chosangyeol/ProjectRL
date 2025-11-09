using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace spawn
{
    public class Spawn : MonoBehaviour
    {
        public GameObject obj;
        public int count = 10;
        public Vector2 size = new Vector2(50f, 50f);
        public LayerMask groundMask;

        public float rayHeight = 100f;

        void Start()
        {
            SpawnObjects();
        }

        void SpawnObjects()
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 pos;
                if (TryGetGroundPosition(out pos))
                {
                    Instantiate(obj, pos, Quaternion.identity);
                }
            }
        }

        bool TryGetGroundPosition(out Vector3 hitPos)
        {
            float x = Random.Range(-size.x / 2f, size.x / 2f);
            float z = Random.Range(-size.y / 2f, size.y / 2f);
            Vector3 origin = new Vector3(x, rayHeight, z) + transform.position;

            Ray ray = new Ray(origin, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundMask))
            {
                hitPos = hit.point;
                return true;
            }

            hitPos = Vector3.zero;
            return false;
        }
    }
}
