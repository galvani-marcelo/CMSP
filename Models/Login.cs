using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSP.Models
{
    public class Login
    {
        public string realm { get; set; }
        public string platform { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }

    public class Initial
    {
        public string nick { get; set; }
        public string name { get; set; }
        public string device_id { get; set; }
        public string password { get; set; }
        public string platforms { get; set; }
        public string realm { get; set; }
        public string serverIP { get; set; }
        public string config { get; set; }
        public string session { get; set; }
        public string files_base_url { get; set; }
        public string acs_server { get; set; }
        public string ppcs_server { get; set; }
        public string room_server { get; set; }
        public string msg_drop_rate { get; set; }
        public string msg_per_day { get; set; }
        public string role { get; set; }
        public string lifes { get; set; }
        public string title { get; set; }
        public string assets { get; set; }
        public string webview_url { get; set; }
        public string tms_spa { get; set; }
        public string auth_token { get; set; }
    }

}
