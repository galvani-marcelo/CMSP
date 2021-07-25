using System;


namespace CMSP
{
    using Models;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    class Program
    {

        static string format_username(String Value)
        {
            string _filterRa = string.Empty;
            switch(Value.Length)
            {
                case 10:
                    _filterRa = "000000" + Value;
                    break;
                case 11:
                    _filterRa = "00000" + Value;
                    break;
                case 12:
                    _filterRa = "0000" + Value;
                    break;
                case 13:
                    _filterRa = "000" + Value;
                    break;
                case 14:
                    _filterRa = "00" + Value;
                    break;
                case 15:
                    _filterRa = "0" + Value;
                    break;
                default:
                    _filterRa = Value;
                    break;
            }
            return _filterRa;
        }
        
        static async Task Main(string[] args)
        {
        userinvalid:
            List<string> Text = new List<string>();
            Console.WriteLine("Gabarito do Centro de Mídias!");
            Console.WriteLine("\n");
            Login login = new Login { realm = "edusp", platform = "webclient" };
            Console.WriteLine("Digite seu RA:");
            string RA = Console.ReadLine();
            Console.WriteLine("Digite o UF:");
            string UF = Console.ReadLine();
            Console.WriteLine("Insira ó codigo de acesso web:");
            string codigo = Console.ReadLine();
            Console.WriteLine("");
            login.username = format_username(RA + UF);
            login.password = codigo;

            using (var ApiCMSP = new HttpClient())
            {
                var result =string.Empty;
                ApiCMSP.BaseAddress = new System.Uri("https://cmspweb.ip.tv/");
                string CookieSession = string.Empty;
                string json = JsonSerializer.Serialize(login);
                var data_login = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await ApiCMSP.PostAsync("", data_login);

                IEnumerator<String> ecookie = response.Headers.GetValues("set-cookie").GetEnumerator();

                if (ecookie.MoveNext()) 
                    CookieSession = ecookie.Current.Split(";")[0];

                if (CookieSession == "session=")
                {
                    Console.Clear();
                    goto userinvalid;
                }

                // POST GETINITIAL
                List<KeyValuePair<string, string>> PairInitial = new List<KeyValuePair<string, string>>();
                PairInitial.Add(new KeyValuePair<string, string>("data[action]", "getInitial"));
                PairInitial.Add(new KeyValuePair<string, string>("data[newTab]", "true"));
                PairInitial.Add(new KeyValuePair<string, string>("cookie", CookieSession));
                var content = new FormUrlEncodedContent(PairInitial);
                content.Headers.Clear();
                content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                response = await ApiCMSP.PostAsync("g/getInitial", content);
                result = await response.Content.ReadAsStringAsync();
                Initial initial = JsonSerializer.Deserialize<Initial>(result);

                result = ApiCMSP.PostAsync("g/getRoomsList",content).Result.Content.ReadAsStringAsync().Result;
                Rooms rooms = JsonSerializer.Deserialize<Rooms>(result);
                Console.WriteLine("Aluno: " + initial.name + "\n");

                Console.WriteLine("Salas : \n");
                int count = -1;
                foreach (Room room in rooms.data.rooms)
                {
                    count++;
                    Console.WriteLine("  ["+count.ToString()+"] "+ room.topic);
                }

                Console.WriteLine("\nSelecione uma sala: ");
                int index_room = int.Parse(Console.ReadLine());
                string select_room = rooms.data.rooms[index_room].name;
                List<CTask> tasks = new List<CTask>();
                ApiCMSP.DefaultRequestHeaders.Add("x-api-key", initial.auth_token);

                string room_publicid = String.Format("room/detail/{0}?fields[]=id&fields[]=name&fields[]=category_id",select_room);
                result = await ApiCMSP.GetAsync("https://edusp-api.ip.tv/"+ room_publicid).Result.Content.ReadAsStringAsync();
                PublicId pid = JsonSerializer.Deserialize<PublicId>(result);

                string tms = string.Format("tms/task?type=task&publication_target[]={0}&publication_target[]={1}&limit=100&offset=0&without_answer=true", select_room,pid.category_id.ToString());
                result = await ApiCMSP.GetAsync("https://edusp-api.ip.tv/" + tms).Result.Content.ReadAsStringAsync();
                tasks.AddRange(JsonSerializer.Deserialize<List<CTask>>(result));
                

                Text.Add("<HTML>\n<HEAD>\n<meta charset=\u0022UTF-8\u0022/><TITLE>Gabarito Centro de Mídias</TITLE >\n</HEAD >\n<Body>");
                foreach (CTask task in tasks)
                {
                    Text.Add("<hr/>");
                    Text.Add("<h1>" + task.title+ "</h1>");
                    Text.Add("<p>" + task.description + "</p>");
                    result = await ApiCMSP.GetAsync(String.Format("https://edusp-api.ip.tv/tms/task/{0}?with_questions=true", task.id)).Result.Content.ReadAsStringAsync();
                    CTask task_question = JsonSerializer.Deserialize<CTask>(result);

                    foreach (Question question in task_question.questions)
                    {
                        if (question.type == "single" || question.type == "multi")
                        {
                            Text.Add("<div style=\u0022margin-top: 50px;\u0022>");
                            Text.Add("<h2>" + question.statement + "</h2>");
                            Dictionary<string, Option> options = JsonSerializer.Deserialize<Dictionary<string, Option>>(question.options.ToString());

                            Text.Add("<ul>");
                            foreach (Option option in options.Values)
                            {

                                if (option.answer == true)
                                {
                                    Text.Add("<li>" + option.statement + "✔️</li></br>");
                                }
                                else
                                {
                                    Text.Add("<li>" + option.statement + "❌</li></br>");
                                }

                            }
                            Text.Add("<ul>");
                            Text.Add("</div>");
                        }
                    }
                   
                }
                Text.Add("</body></html >");
            }
            System.IO.File.WriteAllLines("CMSPResposta.html", Text.ToArray());
            Console.WriteLine("");
            Console.WriteLine("Gabarito gerado!");
            Console.ReadKey();
        }
    }
}
