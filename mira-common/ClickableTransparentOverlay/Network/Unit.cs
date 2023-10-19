using System;
using System.Net;

namespace Common
{
    [Serializable]
    public class Unit
    {
        public Unit(string Command, object[] Parameters)
        {
            this.Command = Command;
            if (Parameters != null) this.prms = Parameters;
        }

        public bool IsAnswer;
        public bool IsSync;
        public bool IsEmpty = true;
        public readonly string Command;
        public object ReturnValue;
        public object[] prms;
        public Exception Exception;
    }

    [Serializable]
    public class Answer
    {
        public bool IsKeyValid;
        public bool IsNeedUpdate;
        public string AuthKey;
        public long Content;
    }
}
