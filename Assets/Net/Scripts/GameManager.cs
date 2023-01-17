using UnityEngine;
using UnityEngine.InputSystem;

namespace NetGame
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private InputAction _quit;

        private void Start()
        {
            _quit.performed += onQuit;
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
