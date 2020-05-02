using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardsPopUp : MonoBehaviour
{
    public GameObject ScoreHolder;
    public GameObject NoScore;
    public GameObject LeaderboardItem;

    public void SetValues(List<PlayerLeaderboardEntry> results)
    {
        ScoreHolder.transform.DeleteChildren();

        if (GameManager.Instance.playerData.isDataSet)
        {
            for (int i = 0; i < results.Count; i++)
            {
                var newItem = Instantiate(LeaderboardItem, Vector3.zero, Quaternion.identity, ScoreHolder.transform);
                newItem.GetComponent<LeaderboardItem>().SetValues(i + 1, results[i].DisplayName, results[i].StatValue);
            }

            ScoreHolder.SetActive(true);
            NoScore.SetActive(false);
        }
        else
        {
            ScoreHolder.SetActive(false);
            NoScore.SetActive(true);
        }
    }
}