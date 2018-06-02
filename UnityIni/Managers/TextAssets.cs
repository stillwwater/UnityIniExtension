using UnityEngine;

namespace UnityIni.Managers
{
    /// <summary>
    /// Holds default versions of config files
    /// Assign .ini.text files to the textAssets array in the inspector
    /// </summary>
    class TextAssets : MonoBehaviour
    {
        public TextAsset[] textAssets;

        static TextAssets instance;

        public static TextAssets Instance {
            get { return instance; }
        }

        public static TextAsset Find(string name) {
            foreach (var asset in Instance.textAssets) {
                if (asset.name == name) {
                    return asset;
                }
            }
            return null;
        }

        void Awake() {
            if (instance == null) {
                instance = this;
            } else {
                Destroy(gameObject);
            }
        }
    }
}
