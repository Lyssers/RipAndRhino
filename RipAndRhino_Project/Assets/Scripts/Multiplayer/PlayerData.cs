using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public string id;
    public string username;
    public int bestScore;
    public DateTime date;
    public int totalPlayersInTheGame;
    public string roomName;
    public bool isDataSet;

    public PlayerData()
    {
        id = Guid.NewGuid().ToString();
    }
}