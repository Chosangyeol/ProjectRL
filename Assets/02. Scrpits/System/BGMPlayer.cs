using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    private AudioSource audioS;
    [Header("일반 BGM")]
    public AudioClip defaultBGM;
    [Header("보스 BGM")]
    public AudioClip bossBGM;

    private void Awake()
    {
        audioS = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (defaultBGM != null) return;

        audioS.clip = defaultBGM;
        audioS.Play();
    }

    public void BossBgmPlay()
    {
        if (bossBGM != null) return;

        audioS.Stop();
        audioS.clip = bossBGM;
        audioS.Play();
    }
}
