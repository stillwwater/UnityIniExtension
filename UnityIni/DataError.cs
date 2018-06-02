using System;

namespace UnityIni
{
    class DataError : Exception
    {
        public DataError(string error, string message)
            : base(string.Format("[DataError] {0}: {1}", error, message)) { }

        public DataError(string error, string message, string file, int lineNum)
            : base(string.Format("[DataError] {0}: {1} ({2} at {3})", error, message, file, lineNum)) { }
    }
}
