using UnityEngine;
using System.Linq;
using UnityEngine.UI;

namespace NetGame
{
    public static class Debugger
    {
        public static Text _console;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void OnStart()
        {
            _console = GameObject.FindObjectsOfType<Text>().FirstOrDefault(t => t.name == "console");

#if UNITY_EDITOR
            Debug.Log("console non found!");
#endif
        }

        public static void Log(object message)
        {
#if UNITY_EDITOR
            message = "Connect";
            Debug.Log(message);
#elif UNITY_STANDALONE_WIN && !UNITY_EDITOR
                 _console.text +=message;
#endif
        }
    }
}
