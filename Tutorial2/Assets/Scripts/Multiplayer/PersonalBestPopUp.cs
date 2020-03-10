using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonalBestPopUp : MonoBehaviour
{
    public GameObject ScoreHolder;
    public GameObject NoScore;
    public Text Username;
    public Text BestScore;
    public Text Date;
    public Text TotalPlayers;
    public Text RoomName;
    // Start is called before the first frame update
    private void OnEnable()
    {
        SetValues();
    }

    private void SetValues()
    {
        var playerData = GameManager.Instance.playerData;
        if(playerData == null)
        {
            ScoreHolder.SetActive(false);
            NoScore.SetActive(true);
        }
        else
        {
            Username.text = playerData.username;
            BestScore.text = playerData.bestScore.ToString();
            Date.text = playerData.date.ToString();
            TotalPlayers.text = playerData.totalPlayersInTheGame.ToString();
            RoomName.text = playerData.roomName;

            ScoreHolder.SetActive(true);
            NoScore.SetActive(false);
        }
    }
}
