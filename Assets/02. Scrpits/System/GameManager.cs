using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private float timer = 0;
    public float Timer => timer;

    private int money = 30;
    public int Money => money;

    private int difficulty = 0;
    public int Difficulty => difficulty;    

    public event Action<int> OnMoneyChange;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        difficulty = PlayerPrefs.GetInt("difficulty",0);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
    }

    public void AddMoney(int amount)
    {
        this.money += amount;
        OnMoneyChange?.Invoke(money);
    }

    public void RemoveMoney(int amount)
    {
        this.money -= amount;
        OnMoneyChange?.Invoke(money);
    }
}
