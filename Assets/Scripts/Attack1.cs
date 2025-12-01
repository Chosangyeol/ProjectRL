using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack1 : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 5f;
    public GameObject PoisonPrefab;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            Instantiate(PoisonPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
