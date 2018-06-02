using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

namespace UnityIni
{
    abstract class Config
    {
        static List<Config> configs = new List<Config>();
        static string dataPath = Application.persistentDataPath; // copy for safe use in threading

        object _lock = new object();

        protected List<Config> Configs {
            get {
                lock (_lock) {
                    return Config.configs;
                }
            }
        }

        /// <summary>
        /// Read configuration from disk
        /// </summary>
        public abstract void ReadFromDisk();

        /// <summary>
        /// Save configuration to disk
        /// </summary>
        public abstract void SaveToDisk();

        /// <summary>
        /// Save all loaded configuration files to disk
        /// </summary>
        public static void SaveAll() {
            foreach (var config in configs) {
                config.SaveToDisk();
            }
        }

        /// <summary>
        /// Save all loaded configuration files to disk asynchronously
        /// </summary>
        public static void SaveAllAsync() {
            new Thread(SaveAll).Start();
        }

        /// <summary>
        /// Get safe path to config file using Application.persistentDataPath
        /// </summary>
        public static string GetConfigPath(string directory, string file) {
            return Path.Combine(Path.Combine(dataPath, directory), file);
        }

        protected void AddConfig(Config c) {
            configs.Add(c);
        }
    }
}
