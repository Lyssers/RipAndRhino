using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    public Text EnemyKilledText;
    public Text TotalEnemyCountText;
    public Text TimeToComplete;
    private int _enemyKilledCount;
    private float _totalTime;
    private void Start()
    {
        _enemyKilledCount = 0;
        _totalTime = LevelManager.Instance.TotalTimeToComplete;
        if(!LevelManager.Instance.LimitTime)
        {
            TimeToComplete.text = "-";
        }
        TotalEnemyCountText.text = LevelManager.Instance.EnemiesCount.ToString();
    }

    private void OnEnable()
    {
        Enemy.OnEnemyKilled += EnemyKilled;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyKilled -= EnemyKilled;
    }

    private void EnemyKilled()
    {
        _enemyKilledCount++;
        EnemyKilledText.text = _enemyKilledCount.ToString();
    }

    private void Update()
    {
        if(LevelManager.Instance.LimitTime)
        {
            _totalTime -= Time.deltaTime;
            TimeToComplete.text = _totalTime.ToString("0.00");
            if(_totalTime <= 0)
            {
                LevelManager.Instance.Player.UnitDied();
                TimeToComplete.text = "0.00";
            }
        }
    }
}
