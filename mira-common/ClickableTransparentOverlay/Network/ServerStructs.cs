using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class BaseResponce
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class StringDataResponce : BaseResponce
    {
        public string Data { get; set; }
    }

    public class BaseRequest
    {
        public string Method { get; set; }
        public string Token { get; set; }
        public string Game { get; set; }
    }

    public class LogErrorRequest : BaseRequest
    {
        public string Message { get; set; }
    }

    public class UpdateRequest : BaseRequest
    {
        public string ExeName;
        public bool UseExperementalVersion;
        public string UpdaterVersion;
    }

    public class UpdateResponce : BaseResponce
    {
        public UpdateData Data;
    }
}
