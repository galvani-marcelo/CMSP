using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSP.Models
{
    public class CTask
    {
        public int id { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string author { get; set; }
        public string publication_target { get; set; }
        public DateTime publish_at { get; set; }
        public DateTime expire_at { get; set; }
        public List<string> tags { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int parent_id { get; set; }
        public object style { get; set; }
        public string realm { get; set; }
        public string publication { get; set; }
        public List<Question> questions { get; set; }
    }


    public class Question
    {
        public int id { get; set; }
        public int task_id { get; set; }
        public string type { get; set; }
        public int order { get; set; }
        public string statement { get; set; }
        public string media_type { get; set; }
        public string media_url { get; set; }
        public bool required { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int parent_id { get; set; }
        public int? score { get; set; }
        public object options { get; set; }
    }

    public class Option
    {
        public string id { get; set; }
        public bool answer { get; set; }
        public bool loading { get; set; }
        public string media_url { get; set; }
        public bool showImage { get; set; }
        public string statement { get; set; }
        public string media_type { get; set; }
        public bool showFunction { get; set; }
    }

}
