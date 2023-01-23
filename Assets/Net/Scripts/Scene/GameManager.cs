using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace NetGame
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        private PlayerController _player1;
        private PlayerController _player2;
        private Camera _camera;

        [SerializeField] private string _prefabPlayersnames;
        [SerializeField] private InputAction _quit;

        [Space, SerializeField, Range(1f, 15f)] private float _randomInterval = 10f;

        private void Start()
        {
            _quit.Enable();
            _quit.performed += onQuit;
            _camera = Camera.main;

            var positionPlayer = new Vector3(Random.Range(-_randomInterval, _randomInterval), 1f, Random.Range(-_randomInterval, _randomInterval));
            var GO = PhotonNetwork.Instantiate(_prefabPlayersnames + PhotonNetwork.NickName, positionPlayer, new Quaternion());
            _camera.transform.parent = GO.transform;
            _camera.transform.localPosition = new Vector3(0f, 2f, -2f);

            PhotonPeer.RegisterType(typeof(PlayerData), 100, Debugger.SerializePlayerData, Debugger.DeserializePLayerData);
        }

        public void AddPLayer(PlayerController player)
        {
            if (player.name.Contains("1")) _player1 = player;
            else _player2 = player;

            if (_player1 != null && _player2 != null)
            {
                _player1.SetTarget(_player2.transform);
                _player2.SetTarget(_player1.transform);
            }


        }

        private void onQuit(InputAction.CallbackContext obj)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE_WIN && !UNITY_EDITOR
            Application.Quit();
#endif

            PhotonNetwork.LeaveRoom();
        }
    }
}
