using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetStartPoint : MonoBehaviour
{
    private void Start()
    {
        SetPlayerSpawnPoint();
    }

    private void SetPlayerSpawnPoint()
    {
        GameObject player = GameObject.FindWithTag("Player");
        GameObject spawnPos = GameObject.FindWithTag("StartPoint");

        player.transform.position = spawnPos.transform.position;
        player.transform.rotation = spawnPos.transform.rotation;
    }
}
