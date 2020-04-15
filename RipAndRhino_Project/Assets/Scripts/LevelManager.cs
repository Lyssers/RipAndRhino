using RhinoGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public GameOver GameOverCanvas;
    public Transform Enemies;
    public Player Player;
    [HideInInspector]
    public int EnemiesCount;
    private int _enemiesKilled;
    [Header("Time to complete the level")]
    public bool LimitTime;
    public float TotalTimeToComplete;
    void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        Enemy.OnEnemyKilled += EnemyKilled;
        EnemiesCount = Enemies.childCount;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyKilled -= EnemyKilled;
    }

    private void EnemyKilled()
    {
        _enemiesKilled++;
        if(_enemiesKilled == EnemiesCount)
        {
            GameOverCanvas.ShowGameOver("You won!");
        }
    }
}
