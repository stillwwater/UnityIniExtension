using System.Collections.Generic;

namespace UnityIni
{
    class Ini
    {
        class Env
        {
            public const char DEF = '=';
            public const char NEW_LINE = '\n';
            public const char COMMENT = ';';
            public const char OPEN_TAG = '[';
            public const char CLOSE_TAG = ']';
        }

        string name;

        public Ini(string name) {
            this.name = name;
        }

        /// <summary>
        /// Returns heading for this parser plus a message
        /// </summary>
        public string GetHeading(string message) {
            return string.Format("; @@ fast_ini v0.1, a{0}; {1}{0}", Env.NEW_LINE, message);
        }

        /// <summary>
        /// Compiles dictionary to ini text, line by line
        /// </summary>
        public IEnumerable<string> Compile(Dictionary<string, Dictionary<string, string>> input) {
            foreach (var tkPair in input) {
                yield return string.Format("[{0}]{1}", tkPair.Key, Env.NEW_LINE);
                foreach (var kvPair in tkPair.Value) {
                    yield return string.Format("{0}={1}{2}", kvPair.Key, kvPair.Value, Env.NEW_LINE);
                }
            }
        }

        /// <summary>
        /// Parses ini text
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> Parse(string text) {
            return Parse(text.Split(Env.NEW_LINE));
        }

        /// <summary>
        /// Parses ini lines
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> Parse(string[] lines) {
            var tree = new Dictionary<string, Dictionary<string, string>>();

            // setup global scope
            string tag = "__global__";
            tree.Add(tag, new Dictionary<string, string>());

            for (int i = 0; i < lines.Length; i++) {
                string input = lines[i].Trim();

                if (input == "" || input[0] == Env.COMMENT) {
                    // skip comments and empty lines
                    continue;
                }

                if (input[0] == Env.OPEN_TAG) {
                    // parse tags
                    tag = ParseTag(input);

                    if (!tree.ContainsKey(tag) && tag != "") {
                        tree.Add(tag, new Dictionary<string, string>());
                    }
                    continue;
                }

                // input is not a tag or comment so it must be a new key, value pair
                var pair = ParseDefinition(input, i);
                tree[tag].Add(pair.Key, pair.Value);
            }

            return tree;
        }

        KeyValuePair<string, string> ParseDefinition(string input, int lineNum) {
            string[] pair = input.Split(Env.DEF);

            if (input[input.Length - 1] == Env.DEF) {
                // Key without a value (allowed)
                return new KeyValuePair<string, string>(pair[0].Trim(), null);
            }

            if (pair.Length != 2) {
                throw new DataError("Ini ParserError", "Illegal instruction", this.name, lineNum);
            }

            return new KeyValuePair<string, string>(pair[0].Trim(), pair[1].Trim());
        }

        string ParseTag(string input) {
            return input[input.Length - 1] == Env.CLOSE_TAG
                    ? input.Substring(1, input.Length - 2)
                    : input.Substring(1);
        }
    }
}
