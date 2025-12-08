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
    public Slider expSlider;
    public TMP_Text lvText;

    public TMP_Text timeText;

    private float time;

    private void Awake()
    {
        _model = GameObject.FindAnyObjectByType <PlayerModel>();
        UpdateHp(_model);
    }

    private void Update()
    {
        time = GameManager.Instance.Timer;

        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

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

    public void UpdateExp(PlayerModel model)
    {
        expSlider.maxValue = model.Stat.Stat.expMax;
        expSlider.value = model.Stat.Stat.expCurrent;
        lvText.text = "Lv. " + model.Stat.Stat.levelCurrent.ToString();
    }
}
