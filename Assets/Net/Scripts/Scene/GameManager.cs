using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NetGame
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private string _prefabPlayersnames;
        [SerializeField] private InputAction _quit;

        [Space, SerializeField, Range(1f, 15f)] private float _randomInterval = 10f;

        private void Start()
        {
            _quit.performed += onQuit;

            var positionPlayer = new Vector3(Random.Range(-_randomInterval, _randomInterval), 1f, Random.Range(-_randomInterval, _randomInterval));
            var GO = PhotonNetwork.Instantiate(_prefabPlayersnames + PhotonNetwork.NickName, positionPlayer, new Quaternion());
        }

        private void onQuit(InputAction.CallbackContext obj)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE_WIN && !UNITY_EDITOR
            Application.Quit();
#endif
        }
    }
}
