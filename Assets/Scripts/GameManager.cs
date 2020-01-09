﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public int level = 1;
    public int food = 100;
    public AudioClip dieClip;

    [HideInInspector]public List<Enemy> enemyList = new List<Enemy>();
    [HideInInspector]public bool isEnd = false;
    private bool sleepStep = true;
    private Text foodText;
    private Text failText;
    private Image dayImage;
    private Text dayText;
    private Player player;
    private MapManager mapManager;

    // Use this for initialization
    void Awake () {
        _instance = this;
        DontDestroyOnLoad(gameObject);
        InitGame();
	}

    void InitGame()
    {
        mapManager = GetComponent<MapManager>();
        mapManager.InitMap();

        foodText = GameObject.Find("FoodText").GetComponent<Text>();
        UpdateFoodText(0);
        failText = GameObject.Find("FailText").GetComponent<Text>();
        failText.enabled = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        dayImage = GameObject.Find("DayImage").GetComponent<Image>();
        dayText = GameObject.Find("DayText").GetComponent<Text>();
        dayText.text = "Day " + level;
        Invoke("HideBlack", 1);

        isEnd = false;
        enemyList.Clear();
    }

    void UpdateFoodText(int foodChange)
    {
        if (foodChange == 0)
        {
            foodText.text = "Food:" + food;
        }
        else
        {
            string str = "";
            if (foodChange < 0)
            {
                str = foodChange.ToString();
            }
            else
            {
                str = "+" + foodChange;
            }
            foodText.text = str + "   Food:" + food;
        }
    }

    public void ReduceFood(int count) {
        food -= count;
        UpdateFoodText(-count);
        if (food <= 0)
        {
            failText.enabled = true;
            AudioManager.Instance.RandomPlay(dieClip);
            AudioManager.Instance.StopBGM();
        }
    }
	public void AddFood(int count) {
        food += count;
        UpdateFoodText(count);
    }

    public void OnPlayerMove()
    {
        if (sleepStep == true)
        {
            sleepStep = false;
        }
        else
        {
            foreach(var enemy in enemyList)
            {
                enemy.Move();
            }
            sleepStep = true;
        }//检测是否到达终点
        if(player.targetPos.x==mapManager.cols-2&& player.targetPos.y == mapManager.rows - 2)
        {
            isEnd = true;
            //下一关
            Application.LoadLevel(Application.loadedLevel);//重新加载本关卡
        }
    }
    void OnLevelWasLoaded(int sceneLevel)
    {
        level++;
        InitGame();//初始化游戏
    }

    void HideBlack()
    {
        dayImage.gameObject.SetActive(false);
    }
}

