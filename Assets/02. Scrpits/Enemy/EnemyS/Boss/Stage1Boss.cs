using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Boss : BossBase
{
    private bool isFirstDrop = false;
    private bool isSecondDrop = false;

    public GameObject warningPrefab;
    public GameObject fallingPrefab;

    public float warningDuration = 2f;
    public float fallHeight = 20f;
    public float fallSpeed = 25f;

    private void Start()
    {
        attackBehavior = new Stage1BossAttack();
    }

    public override void TakeDamage(float amount)
    {
        Stat.curHp -= amount;
        if (Stat.curHp < (Stat.totalHp * 0.8f) && !isFirstDrop)
        {
            Debug.Log("Ã¹¹øÂ° ³«¼®");
            isFirstDrop = true;
            StartDrop();
        }
        if (Stat.curHp < (Stat.totalHp * 0.7f) && !isSecondDrop)
        {
            Debug.Log("µÎ¹øÂ° ³«¼®");
            isSecondDrop = true;
            StartDrop();
        }

    }

    private void StartDrop()
    {
        if (player == null) return;

        Vector3 pos = player.position + new Vector3(0, 0.1f, 0);
        
        StartCoroutine(DropActive(pos));
    }

    private IEnumerator DropActive(Vector3 pos)
    {
        GameObject warning = Instantiate(warningPrefab, pos, Quaternion.identity);

        yield return new WaitForSeconds(warningDuration);

        Vector3 spawnPos = pos + Vector3.up * fallHeight;
        GameObject falling = Instantiate(fallingPrefab, spawnPos, Quaternion.identity);

        Rigidbody rb = falling.GetComponent<Rigidbody>();
        if (rb == null) rb = falling.AddComponent<Rigidbody>();

        rb.useGravity = false;

        while (falling.transform.position.y > pos.y + 0.5f)
        {
            falling.transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            yield return null;
        }

        falling.transform.position = new Vector3(
            pos.x,
            pos.y,
            pos.z
        );

        Destroy(rb);
        falling.transform.rotation = Quaternion.identity;

        Destroy(warning);
    }
}
