using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Generic.API.ViewModel
{
    public class PushMessageViewModel
    {
        public string Message { get; set; }
        public String DeviceToken { get; set; }
        public int Type { get; set; }
    }
}