using Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainUIManager : MonoBehaviour
{
    private PlayerModel _model;

    public Slider hpSlider;
    public TMP_Text hpText;
    
    public TMP_Text timeText;

    // 추후 GameManager로 이동
    private float time;

    private void Awake()
    {
        _model = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).GetComponent<PlayerModel>();
        UpdateHp(_model);
    }

    private void Update()
    {
        time += Time.deltaTime;

        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        // 00:00 형식으로 표시
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void OnEnable()
    {
        _model.ActionCallbackStatChanged += UpdateHp;
    }

    private void OnDisable()
    {
        _model.ActionCallbackStatChanged -= UpdateHp;
    }

    public void UpdateHp(PlayerModel model)
    {
        hpSlider.maxValue = model.Stat.Stat.hpMax;
        hpSlider.value = model.Stat.Stat.hpCurrent;
        hpText.text = model.Stat.Stat.hpCurrent + " / " + model.Stat.Stat.hpMax;
    }
}
