using System.Collections;

using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using System;

namespace RhinoGame
{
    public class MultiplayerLevelManager : MonoBehaviourPunCallbacks
    {
        public static MultiplayerLevelManager Instance = null;
        public int MaxScore = 5;
        public GameObject GameOverPanel;
        public Text InfoText;
        public Transform[] PlayerPositions;
        [Header("Timer")]
        public GameObject TimerPanel;
        public Text TimerText;
        public bool HasTimeLimit;
        public float TimeLimit;

        public void Awake()
        {
            Instance = this;
        }

        public void Start()
        {
            StartGame();
            StartCoroutine(StartTimer());
        }

        private IEnumerator StartTimer()
        {
            if (HasTimeLimit)
            {
                TimerPanel.SetActive(true);
                while (TimeLimit > 0)
                {
                    //yield return new WaitForEndOfFrame();
                    TimerText.text = TimeLimit.ToString("n1");
                    yield return new WaitForEndOfFrame();
                    TimeLimit -= Time.deltaTime;
                }
                FindWinner();
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MultiplayerLobby");
        }

        public override void OnLeftRoom()
        {
            PhotonNetwork.Disconnect();
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                PhotonNetwork.Disconnect();
            }
        }

        private void StartGame()
        {
            //int stuff = PhotonNetwork.LocalPlayer.GetPlayerNumber();
            var position = PlayerPositions[PhotonNetwork.LocalPlayer.GetPlayerNumber()].position;
            PhotonNetwork.Instantiate("Rhino", position, Quaternion.identity, 0);
        }

        public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
        {
             if (changedProps.ContainsKey("score"))
            {
                CheckEndOfGame();
            }
        }

        private void CheckEndOfGame()
        {
            // If HasTimeLimit is set to true then we immediately return as we don't want to check if any player has reached max number of kills
            if (HasTimeLimit)
            {
                return;
            }
            bool showGameOver = false;
            
            foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList)
            {
                int stuff = p.GetScore();
                if (p.GetScore() >= MaxScore)
                {
                    showGameOver = true;
                    break;
                }
            }

            if (showGameOver)
            {
                FindWinner();
            }
        }

        private void FindWinner()
        {
            string winner = "";
            int score = -1;
            Color color = Color.black;

            foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList)
            {
                if (p.GetScore() > score)
                {
                    winner = p.NickName;
                    score = p.GetScore();
                    color = AsteroidsGame.GetColor(p.GetPlayerNumber());
                }
            }

            StartCoroutine(EndOfGame(winner, score, color));
            StorePersonalBest();
        }

        private void StorePersonalBest()
        {
            var username = PhotonNetwork.LocalPlayer.NickName;
            var score = PhotonNetwork.LocalPlayer.GetScore();

            PlayerData playerData = GameManager.Instance.playerData;
            if (playerData != null)
            {
                if (score >= playerData.bestScore)
                {
                    //Set new best
                    playerData.username = username;
                    playerData.bestScore = score;
                    playerData.date = DateTime.UtcNow;
                    playerData.totalPlayersInTheGame = PhotonNetwork.CurrentRoom.PlayerCount;
                    playerData.roomName = PhotonNetwork.CurrentRoom.Name;
                }
            }
            else
            {
                // Set current score as a new best
                playerData = new PlayerData()
                {
                    username = username,
                    bestScore = score,
                    date = DateTime.UtcNow,
                    totalPlayersInTheGame = PhotonNetwork.CurrentRoom.PlayerCount,
                    roomName = PhotonNetwork.CurrentRoom.Name
                };
            }

            GameManager.Instance.playerData = playerData;
            GameManager.Instance.SavePersonalBest();
        }

        private IEnumerator EndOfGame(string winner, int score, Color color)
        {
            GameOverPanel.SetActive(true);
            float timer = 20.0f;

            while (timer > 0.0f)
            {
                InfoText.color = color;
                InfoText.text = string.Format("Player {0} won with {1} points.\n\n\nStart another round or leave {2} seconds remaining.", winner, score, timer.ToString("n2"));

                yield return new WaitForEndOfFrame();

                timer -= Time.deltaTime;
            }

            PhotonNetwork.LeaveRoom();
        }

    }
}