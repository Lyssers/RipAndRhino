using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Text TextDescription;
    // Start is called before the first frame update
    public void ShowGameOver(string description)
    {
        LevelManager.Instance.LimitTime = false;
        TextDescription.text = description;
        gameObject.SetActive(true);
    }
}
