using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSP.Models
{
    public class Room
    {
        public string name { get; set; }
        public string topic { get; set; }
        public List<string> oper { get; set; }
    }

    public class Config
    {
        public int msg_per_day { get; set; }
        public int msg_delay { get; set; }
        public bool attachment_disabled { get; set; }
        public int tx_duration { get; set; }
        public int tx_interval { get; set; }
    }

    public class Data
    {
        public List<Room> rooms { get; set; }
        public List<string> server { get; set; }
        public Config config { get; set; }
    }

    public class Rooms
    {
        public string result { get; set; }
        public Data data { get; set; }
    }

    public class PublicId
    {
        public int id { get; set; }
        public string name { get; set; }
        public int category_id { get; set; }
    }
}
