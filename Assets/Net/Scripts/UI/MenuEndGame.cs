using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NetGame
{
    public class MenuEndGame : MonoBehaviour
    {
        [SerializeField] private GameObject _panelStatus;
        [SerializeField] private Text _textEndGame;

        public void Update()
        {
            if (PlayerController.instance.HealthPlayer <= 0)
            {
                StartCoroutine(CloseGameRoom());
            }
        }

        private IEnumerator CloseGameRoom()
        {
            _panelStatus.SetActive(true);
            _textEndGame.text = "Победил" + "Player" + (string)PhotonNetwork.NickName;
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene("NetMenuScene");
        }
    }
}
