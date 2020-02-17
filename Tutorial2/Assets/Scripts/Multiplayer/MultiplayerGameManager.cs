using System.Collections;

using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;

namespace RhinoGame
{
    public class MultiplayerGameManager : MonoBehaviourPunCallbacks
    {
        public static MultiplayerGameManager Instance = null;

        public void Awake()
        {
            Instance = this;
        }

        public void Start()
        {
            StartGame();
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
            if (PhotonNetwork.CurrentRoom.PlayerCount < 1)
            {
                PhotonNetwork.Disconnect();
            }
        }

        private void StartGame()
        {
            PhotonNetwork.Instantiate("Rhino", Vector3.zero, Quaternion.identity, 0);
        }

    }
}