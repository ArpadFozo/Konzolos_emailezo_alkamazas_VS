using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace zhGyakorlas2
{
    public class Worker
    {
        public string email { get; set; }
        public string name { get; set; }
    }
    public class Program
    {
        static string[] urls;
        static XDocument[] xdoc;
        static Task[] tasks;
        static List<Worker>[] workerLists;
        static List<Worker> mainList;
        
        static void Main(string[] args)
        {
            urls = new string[3];
            urls[0] = "http://users.nik.uni-obuda.hu/kovacs.andras/haladoprog/data/bmf.xml";
            urls[1] = "http://users.nik.uni-obuda.hu/kovacs.andras/haladoprog/data/kmf.xml";
            urls[2] = "http://users.nik.uni-obuda.hu/kovacs.andras/haladoprog/data/kimf.xml";

            xdoc = new XDocument[urls.Length];
            tasks = new Task[urls.Length];

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = new Task(Reader, i);
                tasks[i].Start();
            }

            Task.WhenAll(tasks).ContinueWith( t => {

                workerLists = Converter(xdoc);
                mainList = Union(workerLists);
                DirectioryGo(mainList);
                Display(mainList);
            }).Wait();
            Console.ReadLine();
        }

        static void Reader(object value)
        {
            int id = (int)value;
            xdoc[id] = XDocument.Load(urls[id]);
        }
        static List<Worker> [] Converter(XDocument[] xdoc)
        {
            List<Worker>[] returnList = new List<Worker>[xdoc.Length];
            for (int i = 0; i < xdoc.Length; i++)
            {

                returnList[i] = new List<Worker>();
                foreach (var item in xdoc[i].Descendants("person"))
                {
                    Worker tempworker = new Worker()
                    {
                        email = item.Element("email").Value,
                        name = item.Element("name").Value
                    };
                    returnList[i].Add(tempworker);
                }
            }
            return returnList;
        }

        public static List<Worker> Union(List<Worker> [] tempList)
        {
            List<Worker> returnList = new List<Worker> ();

            for (int i = 0; i < tempList.Length; i++)
            {
                for (int j = 0; j < tempList[i].Count; j++)
                {
                    var temp = tempList[i][j];

                    var q = returnList.Where(t => t.name == temp.name).FirstOrDefault();
                    if(q == null)
                    {
                        string oldemail = temp.email.Substring(temp.email.IndexOf('@') + 1, temp.email.LastIndexOf('.') - temp.email.IndexOf('@') - 1);
                        temp.email = temp.email.Replace(oldemail, "bmf");
                        returnList.Add(temp);
                    }
                }
            }

            return returnList;
        }

        static void DirectioryGo(List<Worker> sourceList)
        {
            foreach (var item in sourceList)
            {
                if(!Directory.Exists(Directory.GetCurrentDirectory() + "/" + item.email))
                {
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/" + item.email);
                }
            }
            
        }
        private static string EmailFolderFind()
        {
            string directory = Environment.CurrentDirectory;
            string[] temp = directory.Split(new string[] { "\\" }, StringSplitOptions.None);
            int j = 0;
            while (temp[j] != "zhGyakorlas2")
            {
                j++;
            }
            string sumString = "";
            int z = 1;
            sumString += temp[0];
            while (z <= j)
            {
                sumString += @"\" + temp[z];
                z++;
            }
            string temp2 = @"\EmailSender\bin\Debug\EmailSender.exe";
            return sumString + temp2;
        }
        static void Display(List<Worker> sourceList)
        {
            for (int i = 0; i < sourceList.Count; i++)
            {
                Console.WriteLine("[" + i + "] " + sourceList[i].name);
            }
            Console.Write("\nSend email to: ");
            int emailto = int.Parse(Console.ReadLine());
            var selected = sourceList.ElementAt(emailto);
            string arg = Directory.GetCurrentDirectory() +"/"+selected.email;
            Process p = new Process()
            {
                StartInfo = new ProcessStartInfo(EmailFolderFind(), arg)
                {
                    CreateNoWindow = false,
                    UseShellExecute = true,
                    RedirectStandardOutput = false

                }
            };
            p.Start();
            p.WaitForExit();
            

        }
    }
}
