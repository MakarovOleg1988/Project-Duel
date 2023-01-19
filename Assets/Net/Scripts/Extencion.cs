using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

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

        public static byte[] SerializePlayerData(object data)
        {
            var player = (PlayerData)data;

            var array = new List<byte>(16);

            array.AddRange(BitConverter.GetBytes(player.posX));
            array.AddRange(BitConverter.GetBytes(player.posZ));
            array.AddRange(BitConverter.GetBytes(player.rotY));
            array.AddRange(BitConverter.GetBytes(player.HpPlayer));

            return array.ToArray();
        }

        public static object DeserializePLayerData(byte[] data)
        {
            return new PlayerData
            {
                posX = BitConverter.ToSingle(data, 0),
                posZ = BitConverter.ToSingle(data, 4),
                rotY = BitConverter.ToSingle(data, 8),
                HpPlayer = BitConverter.ToSingle(data, 12)
            };
        }
    }

    public struct PlayerData
    {
        public float posX;
        public float posZ;
        public float rotY;
        public float HpPlayer;


        public static PlayerData Create(PlayerController player)
        {
            return new PlayerData
            {
                posX = player.transform.position.x,
                posZ = player.transform.position.z,
                rotY = player.transform.eulerAngles.y,
                HpPlayer = player._healthPlayer
            };
        }


        public void Set(PlayerController player)
        {
            var vector = player.transform.position;
            vector.x = posX; vector.z = posZ;
            player.transform.position = vector;

            vector = player.transform.eulerAngles;
            vector.y = rotY;
            player.transform.eulerAngles = vector;

            player._healthPlayer = HpPlayer;
        }
    }
}
