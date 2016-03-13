using System;
using System.Net;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using MySql.Data.MySqlClient;

namespace ServerIP
{
    class Program
    {
        static void Main(string[] args)
        {
            QueryHelper.ImportQueries();//Important thing must be first;
            Console.WriteLine("Running async server.");
            new AsyncServer();

        }
    }

    public class AsyncServer
    {
        public AsyncServer()
        {
            var listener = new HttpListener();

            // listener.Prefixes.Add("http://localhost:8081/");
            // listener.Prefixes.Add("http://127.0.0.1:8081/");
            listener.Prefixes.Add("http://192.168.1.30:11000/");

            listener.Start();

            while (true)
            {
                try
                {
                    var context = listener.GetContext();
                    byte[] bytes = new byte[1024];
                    context.Request.InputStream.Read(bytes, 0, bytes.Length);
                    Console.WriteLine("client message:");
                    JObject jo = JObject.Parse(Encoding.UTF8.GetString(bytes).Trim());
                    //jo.GetValue("email"); password, action< login register>
                    Console.WriteLine(jo);
                    ThreadPool.QueueUserWorkItem(o => HandleRequest(context, jo));
                }
                catch (Exception)
                {
                    // Ignored for this example
                }
            }
        }

        private void HandleRequest(object state, JObject jo)
        {
            try
            {
                var context = (HttpListenerContext)state;
                context.Response.StatusCode = 200;
                context.Response.SendChunked = true;

                string response = SearchForUser(jo).ToString();
                Console.WriteLine(response);
                var bytes = Encoding.UTF8.GetBytes(response);
                context.Response.OutputStream.Write(bytes, 0, bytes.Length);
                context.Response.OutputStream.Close();
            }
            catch (Exception)
            {
                Console.WriteLine("asdasd");
                // Client disconnected or some other error - ignored for this example
            }
        }
        private JObject SearchForUser(JObject jo)
        {
            //TODO add constants
            if (jo.GetValue("action").ToString() == "login")
            {
                return Login(jo);
            }
            return Register(jo);
        }

        private JObject Login(JObject jo)
        {
            string t;
            JObject j = new JObject();
            if (!FindUserName(jo.GetValue("email").ToString()))
            {
                t = "fail_email";
                j.Add("result", t);
                return j;
            }
            Query query = new Query();
            //TODO rename functions;
            query.setParameter("userName", jo.GetValue("email").ToString());
            query.setParameter("password", jo.GetValue("password").ToString());
            query.name = "getUser";
            MySqlDataReader user = query.ExecuteSelect();
            user.Read();

            if (user.HasRows)
            {
                //TODO something
                t = "success";
            }
            else t = "fail_password";
            j.Add("result", t);
            return j;
        }

        private JObject Register(JObject jo)
        {
            string t = "success";
            JObject j = new JObject();

            Query query = new Query();
            query.setParameter("userName", jo.GetValue("email").ToString());
            query.setParameter("password", jo.GetValue("password").ToString());
            query.name = "insertUser";
            query.ExecuteNonSelect();

            j.Add("result", t);
            return j;
        }

        private bool FindUserName(string userName)
        {
            Query query = new Query();
            //TODO rename functions;
            query.setParameter("userName", userName);
            query.name = "findUser";
            MySqlDataReader user = query.ExecuteSelect();
            user.Read();
            if (user.HasRows)
                return true;
            return false;
        }
    }
}
/* 
Console.WriteLine(user["user_name"]);
Console.ReadKey();*/
