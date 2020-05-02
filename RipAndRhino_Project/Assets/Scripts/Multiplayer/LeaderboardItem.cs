using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardItem : MonoBehaviour
{
    public Text Order;
    public Text Username;
    public Text BestScore;

    public void SetValues(int order, string username, int score)
    {
        Order.text = order.ToString() + ")";
        Username.text = username;
        BestScore.text = score.ToString();
    }
}