using UnityEngine;
using UnityIni;

namespace UnityIni.Examples
{
    /// <summary>
    /// Example game manager class to hold Global Settings for the game
    /// In a real implementation this class would likely be a singleton
    /// and use DontDestroyOnLoad(gameObject)
    /// </summary>
    class Game : MonoBehaviour
    {
        public class GlobalSettings : IniSerializeable
        {
            public GlobalSettings()
                : base("global.ini") { }
        }

        GlobalSettings settings;

        void Awake() {
            // load global config file
            settings = new GlobalSettings();
            print(settings.Load("Game", "iHighScore"))
        }

        void OnApplicationQuit() {
            // save all config files to disk
            Config.SaveAllAsync();
        }
    }
}
