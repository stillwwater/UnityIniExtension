using UnityEngine;
using UnityIni;

namespace UnityIni.Examples
{
    /// <summary>
    /// Example player class with serializable settings
    /// </summary>
    class Game : MonoBehaviour
    {
        /// <summary>
        /// Stores settings for Player class
        /// Will automatically be serialized and saved to disk
        /// on game exit (see Examples/Game.cs)
        /// </summary>
        class PlayerSettings : IniSerializeable
        {
            public string name;
            public float maxHealth;
            public bool legendary;

            public PlayerSettings()
                : base("player.ini") { }

            public override void Deserialize() {
                base.Deserialize(); // important, loads file data

                // load data
                name = LoadString("__global__", "sName"); // get global setting
                maxHealth = LoadFloat("Status", "fMaxHealth"); // get setting from tag [Status]
                legendary = LoadBool("Status", "bLegendary");
            }

            public override void Serialize() {
                base.Serialize() // important, makes sure data is not null

                // store data
                Store("__global__", "sName", name);
                Store("Status", "fMaxHealth", maxHealth);
                Store("Status", "bLegendary", legendary);
            }
        }

        PlayerSettings settings;

        void Start() {
            // load settings
            // there's no need to call Deserialize as it's called internally
            settings = new PlayerSettings();

            print(settings.name);
            print(settings.maxHealth);
            print(settings.legendary);

            // this will be saved to disk in Application.persistentDataPath
            settings.name = "Player2";
        }
    }
}
