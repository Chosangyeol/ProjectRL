using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EE: MonoBehaviour
{
    public float rotationSpeed = 90f; //회전 속도

    //드론이랑 터렛 위에 떠다니는 오브젝트 회전만 시키는 용도

    void Update()
    {
        // X로 회전
        float rotY = rotationSpeed * Time.deltaTime;
        transform.Rotate(0f, rotY, 0f, Space.Self);
    }

    
}
