using Player;
using Player.Skill;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputSettings;

public class MainUIManager : MonoBehaviour
{
    private PlayerModel _model;

    public Slider hpSlider;
    public TMP_Text hpText;
    public Slider expSlider;
    public TMP_Text lvText;
    public TMP_Text moneyText;

    public TMP_Text timeText;

    public Image[] SkillImage;
    APlayerSkill[] _activeskill;
    private Image[] skillCover = new Image[2];
    private bool[] skillIsOpen = new bool[2];



    private float time;

    private void Awake()
    {
        _model = GameObject.FindAnyObjectByType <PlayerModel>();
        _activeskill = _model.Skill.GetActiveSkill();
    }

    private void Start()
    {
        UpdateHp(_model);
        UpdateMoney(GameManager.Instance.Money);

        skillCover[0] = SkillImage[0].transform.GetChild(1).GetComponent<Image>();
        skillCover[1] = SkillImage[1].transform.GetChild(1).GetComponent<Image>();
        skillIsOpen[0] = false;
        skillIsOpen[1] = false;


        SkillImgChange();
    }

    private void Update()
    {
        time = GameManager.Instance.Timer;

        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (_model.Stat.Stat.levelCurrent < 4)
        {
            skillCover[0].fillAmount = 1f;
        }
        if (_model.Stat.Stat.levelCurrent >= 4 && !skillIsOpen[0])
        {
            skillIsOpen[0] = true;
            skillCover[0].fillAmount = 0f;
        }

        if (_model.Stat.Stat.levelCurrent < 7)
        {
            skillCover[1].fillAmount = 1f;
        }
        if (_model.Stat.Stat.levelCurrent >= 7 && !skillIsOpen[1])
        {
            skillIsOpen[1] = true;
            skillCover[1].fillAmount = 0f;
        }

    }

    private void OnEnable()
    {
        _model.ActionCallbackStatChanged += UpdateHp;
        GameManager.Instance.OnMoneyChange += UpdateMoney;

        
    }

    private void Instance_OnMoneyChange(int obj)
    {
        throw new System.NotImplementedException();
    }

    private void OnDisable()
    {
        _model.ActionCallbackStatChanged -= UpdateHp;
        GameManager.Instance.OnMoneyChange -= UpdateMoney;
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

    public void UpdateMoney(int money)
    {
        moneyText.text = $"{money:0000}G";
    }

    void SkillImgChange()
    {
        for(int i =0; i < SkillImage.Length; i++)
        {
            SkillImage[i].sprite = _activeskill[i].dataSO.skillSprite;
        }
    }


    public IEnumerator SkillCool(int index, float cool)
    {
        float nowTime = 0f;
        skillCover[index].fillAmount = 1f;

        while (nowTime <= cool)
        {
            nowTime += Time.deltaTime;
            skillCover[index].fillAmount = 1f - (nowTime / cool);
            yield return null;
        }

        skillCover[index].fillAmount = 0f;
    }
}
