using System.Collections.Generic;
using System.IO;
using Game.Managers;

namespace UnityIni
{
    class IniSerializeable : Config
    {
        Dictionary<string, Dictionary<string, string>> data;
        string path;
        string name;

        public string Path {
            get { return path; }
        }

        public IniSerializeable(string directory, string file) {
            path = Config.GetConfigPath(directory, file);
            name = file;
            AddConfig(this);
            Deserialize();
        }

        public IniSerializeable(string file)
            : this("", file) { }

        /// <summary>
        /// Load ini from disk and parse it
        /// </summary>
        public override void ReadFromDisk() {
            string text;

            if (!File.Exists(path)) {
                // local file does not exist, load from default
                text = TextAssets.Find(name).text;
            } else {
                text = File.ReadAllText(path);
            }

            if (text == null) {
                return;
            }

            var parser = new Ini(path);
            data = parser.Parse(text);
        }

        /// <summary>
        /// Serialize, compile, and save ini to disk
        /// </summary>
        public override void SaveToDisk() {
            Serialize();
            var parser = new Ini(path);

            using (var sw = new StreamWriter(path, append: false)) {
                sw.Write(parser.GetHeading("backup before modifying"));
                foreach (string line in parser.Compile(data)) {
                    sw.Write(line);
                }
            }
        }

        public virtual void Deserialize() {
            if (data == null) {
                data = new Dictionary<string, Dictionary<string, string>>();
            }
            ReadFromDisk();
        }

        public virtual void Serialize() {
            if (data == null) {
                data = new Dictionary<string, Dictionary<string, string>>();
            }
        }

        public void Destroy() {
            data = null;
        }

        public void Store(string tag, string key, string value) {
            if (!data.ContainsKey(tag)) {
                data.Add(tag, new Dictionary<string, string>());
            }

            if (!data[tag].ContainsKey(key)) {
                data[tag].Add(key, value);
                return;
            }

            data[tag][key] = value;
        }

        public void Store(string tag, string key, bool value) {
            Store(tag, key, value ? "1" : "0");
        }

        public void Store(string tag, string key, object value) {
            Store(tag, key, value.ToString());
        }

        public string LoadString(string tag, string key) {
            return data[tag][key];
        }

        public int LoadInt(string tag, string key) {
            return int.Parse(LoadString(tag, key));
        }

        public float LoadFloat(string tag, string key) {
            return float.Parse(LoadString(tag, key));
        }

        public bool LoadBool(string tag, string key) {
            string b = LoadString(tag, key);
            return b == "1" || b == "t" || b == "yes";
        }
    }
}
