using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalLeaderboard : MonoBehaviour
{
    public int MaxNumberOfResults;
    public void SubmitScore(int playerScore)
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> {
            new StatisticUpdate {
                StatisticName = "Most kills",
                Value = playerScore
                }
            }
        }, OnStatisticsUpdated, OnStatisticsUpdateFailed);
    }

    private void OnStatisticsUpdated(UpdatePlayerStatisticsResult updateResult)
    {
        Debug.Log("Successfully submitted high score");
    }

    private void OnStatisticsUpdateFailed(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with Submitting score. Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    /// <summary>
    /// Get the players with the top 10 high scores in the game
    /// </summary>
    public void RequestLeaderboard()
    {
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StatisticName = "Most kills",
            StartPosition = 0,
            MaxResultsCount = MaxNumberOfResults
        }, OnGetLeaderboard, OnRequestLeaderboardFailed);
    }

    private void OnGetLeaderboard(GetLeaderboardResult result)
    {
        Debug.Log("Leaderboard data fetched!");
        // Show Leaderboard
        GameManager.Instance.LeaderboardsPopUp.GetComponent<LeaderboardsPopUp>().SetValues(result.Leaderboard);
    }

    private void OnRequestLeaderboardFailed(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }
}