using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerIP
{
    public class QueryHelper
    { 
        private static Dictionary<string,Pair<string,List<string>>> queries { get; set; }

        private static void SetParameters(string name, Dictionary<string, string> parameters)
        {
            foreach (var key in parameters.Keys)
            {
                queries[name].Item1 = queries[name].Item1.Replace(queries[name].Item2.Find(x => x.Contains(key)), parameters[key]);
            }
        }
        public static string getQuery(string name, Dictionary<string,string> parameters)
        {
            SetParameters(name, parameters);
            return queries[name].Item1;
        }

        public static void ImportQueries()
        {
            string line;
            string nume = string.Empty;
            string text = string.Empty;
            List<string> parameters = new List<string>();
            System.IO.StreamReader file = new System.IO.StreamReader("E:\\IP\\Server\\ServerIP\\ServerIP\\Queries.txt");
            queries = new Dictionary<string, Pair<string, List<string>>>();

            while ((line = file.ReadLine()) != null)
            {
                if (line.Trim() == "<name>")
                {
                    nume = file.ReadLine();
                    nume = nume.Trim();
                    line = file.ReadLine();
                }
                if (line.Trim() == "<text>")
                {
                    text = file.ReadLine();
                    text = text.Trim();
                    line = file.ReadLine();
                }
                if (line.Trim() == "<parameters>")
                {
                    while (line.Trim() != "</parameters>")
                    {
                        line = file.ReadLine();
                        if (line.Trim() == "<parameter>")
                        {
                            line = file.ReadLine();
                            parameters.Add(line.Trim());
                            line = file.ReadLine();
                        }
                    }
                }
                if (line.Trim() == "</query>")
                {
                    Pair<string, List<string>> queryText = new Pair<string, List<string>>();
                    queryText.Item1 = text;
                    queryText.Item2 = parameters;
                    queries.Add(nume, queryText);
                }
            }
            file.Close();
        }

    }
}
