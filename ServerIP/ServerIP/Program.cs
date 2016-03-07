using System;
using System.Net;
using System.Text;
using System.Threading;

namespace ServerIP
{
    class Program
    {
        static void Main(string[] args)
        {
           
                Console.WriteLine("Running async server.");
                new AsyncServer();
            
        }
    }

    public class SyncServer
    {
        public SyncServer()
        {
            var listener = new HttpListener();

            listener.Prefixes.Add("http://localhost:8081/");
            listener.Prefixes.Add("http://127.0.0.1:8081/");
            listener.Prefixes.Add("http://192.168.1.119:8888/");

            listener.Start();

            while (true)
            {
                try
                {
                    var context = listener.GetContext(); //Block until a connection comes in
                    context.Response.StatusCode = 200;
                    context.Response.SendChunked = true;

                    int totalTime = 0;

                    while (true)
                    {
                        if (totalTime % 3000 == 0)
                        {
                            var bytes = Encoding.UTF8.GetBytes(new string('3', 1000) + "\n");
                            context.Response.OutputStream.Write(bytes, 0, bytes.Length);
                        }

                        if (totalTime % 5000 == 0)
                        {
                            var bytes = Encoding.UTF8.GetBytes(new string('5', 1000) + "\n");
                            context.Response.OutputStream.Write(bytes, 0, bytes.Length);
                        }

                        Thread.Sleep(1000);
                        totalTime += 1000;
                    }

                }
                catch (Exception)
                {
                    // Client disconnected or some other error - ignored for this example
                }
            }
        }
    }

    public class AsyncServer
    {
        public AsyncServer()
        {
            var listener = new HttpListener();

           // listener.Prefixes.Add("http://localhost:8081/");
           // listener.Prefixes.Add("http://127.0.0.1:8081/");
            listener.Prefixes.Add("http://192.168.1.119:80/");

            listener.Start();

            while (true)
            {
                try
                {
                    var context = listener.GetContext();
                    //if(context.Request)
                    byte[] bytes = new byte[1024];
                    context.Request.InputStream.Read(bytes, 0, bytes.Length);
                    string t = Encoding.UTF8.GetString(bytes).Trim();
                    Console.WriteLine("client message:" + t);
                    bytes = Encoding.UTF8.GetBytes(t.Length.ToString());
                    context.Response.OutputStream.Write(bytes, 0, bytes.Length);
                  //  ThreadPool.QueueUserWorkItem(o => HandleRequest(context));
                }
                catch (Exception)
                {
                    // Ignored for this example
                }
            }
        }

        private void HandleRequest(object state)
        {
            try
            {
                var context = (HttpListenerContext)state;

                context.Response.StatusCode = 200;
                context.Response.SendChunked = true;

                int totalTime = 0;

                while (true)
                {
                    if (totalTime % 3000 == 0)
                    {
                        var bytes = Encoding.UTF8.GetBytes(new string('3', 1000) + "\n");
                        context.Response.OutputStream.Write(bytes, 0, bytes.Length);
                    }

                    if (totalTime % 5000 == 0)
                    {
                        var bytes = Encoding.UTF8.GetBytes(new string('5', 1000) + "\n");
                        context.Response.OutputStream.Write(bytes, 0, bytes.Length);
                    }

                    Thread.Sleep(1000);
                    totalTime += 1000;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("asdasd");
                // Client disconnected or some other error - ignored for this example
            }
        }
    }
}
/* QueryHelper.ImportQueries();
Query query = new Query();
query.setParameter("id", "69");
query.name = "getUser";
MySqlDataReader user = query.ExecuteSelect();
user.Read();
Console.WriteLine(user["user_name"]);
Console.ReadKey();*/
