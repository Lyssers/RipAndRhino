using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject PersonalBestPopUp;
    public GameObject LeaderboardsPopUp;
    [SerializeField]
    private string fileName;
    public PlayerData playerData;
    public GlobalLeaderboard GlobalLeaderboard;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        GlobalLeaderboard = GetComponent<GlobalLeaderboard>();
        LoadPersonalBest();
        LoginToPlayFab();
    }

    private void LoginToPlayFab()
    {
        var request = new LoginWithCustomIDRequest { CustomId = playerData.id, CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, x => { Debug.Log("Playfab - Logged In"); }, x => { Debug.Log("Playfab - Login Failed"); });
    }

    // Use this function to generate Random values

    public void SetRandomData()
    {
        playerData = new PlayerData()
       {
            username = "Player " + UnityEngine.Random.Range(1, 5),
            bestScore = UnityEngine.Random.Range(1, 4),
            date = DateTime.UtcNow,
            totalPlayersInTheGame = UnityEngine.Random.Range(1, 5),
            roomName = "Random Room " + UnityEngine.Random.Range(1, 5)
        };
        PrintPlayerData();
    }

    public void SavePersonalBest()
    {
        // Uncomment this line if you want to use Unity's built-in JSON solution
        //var serializedData = JsonUtility.ToJson(playerData);

        // JSON.net - convert the object into a string
        var serializedData = JsonConvert.SerializeObject(playerData);
        var encrypted = AESEcryption.Encrypt(serializedData);

        // Write the string to the file
        File.WriteAllBytes(fileName, encrypted);

        Debug.Log("Data saved!");
    }
    public void LoadPersonalBest()
    {
        try
        {
            // Load the content from the file, if the file does not exist then create it first
            if (!File.Exists(fileName))
            {
                playerData = new PlayerData();
                SavePersonalBest();
                SetRandomData();
            }
            var fileContent = File.ReadAllBytes(fileName);

            var decrypted = "";
            if (fileContent.Length > 0)
            {
                decrypted = AESEcryption.Decrypt(fileContent);
            }

            // JSON.net - convert the string into an object
            playerData = JsonConvert.DeserializeObject<PlayerData>(decrypted);

            // Uncomment this line if you want to print out the playerData object
            PrintPlayerData();
        }
        catch (FileNotFoundException e)
        {
            Debug.Log("File not found: " + e.Message);
        }
        catch (Exception e)
        {
            Debug.Log("Error occured: " + e.Message);
            Debug.Log("Error stack trace: " + e.StackTrace);
        }
    }

    // Use this function to print out the playerData object.

    public void PrintPlayerData()
    {
        if (playerData == null)
        {
            Debug.Log("playerData is null - can't print it!");
            return;
        }
        Debug.Log("**************");
        Debug.Log("username: " + playerData.username);
        Debug.Log("bestScore: " + playerData.bestScore);
        Debug.Log("date: " + playerData.date);
        Debug.Log("totalPlayersInTheGame: " + playerData.totalPlayersInTheGame);
        Debug.Log("roomName: " + playerData.roomName);
        Debug.Log("**************");
    }

    public void ShowPersonalBestPopUp()
    {
        PersonalBestPopUp.SetActive(true);
    }

    public void ShowLeaderboardsPopUp()
    {
        LeaderboardsPopUp.SetActive(true);
        GlobalLeaderboard.RequestLeaderboard();
    }
}