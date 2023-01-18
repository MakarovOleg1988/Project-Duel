using UnityEngine;
using Photon.Pun;

namespace NetGame
{
    public class MenuManager : MonoBehaviourPunCallbacks
    {

        public void CreateRoom_UnityEditor()
        {
            PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 2 });
        }

        public void JoinRoom_UnityEditor()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        public void Quit_UnityEditor()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE_WIN && !UNITY_EDITOR
            Application.Quit();
#endif
        }

        private void Start()
        {
#if UNITY_EDITOR
            PhotonNetwork.NickName = "1";
#elif UNITY_STANDALONE_WIN && !UNITY_EDITOR
            PhotonNetwork.NickName = "2";
#endif

            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = "0.0.1";
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            Debugger.Log("Ready for connection!");
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("NetGameScene");
        }
    }
}
