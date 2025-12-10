using Player;
using Player.Skill;
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


    private float time;

    private void Awake()
    {
        _model = GameObject.FindAnyObjectByType <PlayerModel>();
        
    }

    private void Start()
    {
        UpdateHp(_model);
        UpdateMoney(GameManager.Instance.Money);

        SkillImgChange();
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
        _activeskill = _model.Skill.GetActiveSkill();
        for(int i =0; i < SkillImage.Length; i++)
        {
            SkillImage[i].sprite = _activeskill[i].dataSO.skillSprite;
        }
    }

    public void SkillCoolDownImage(int index, float cooldown)
    {
        for(float i = 0; i < 1; i += Time.deltaTime * cooldown)
        {
            SkillImage[index].color = new Color(i / 255f, i / 255f, i / 255f);
        }
    }
}
