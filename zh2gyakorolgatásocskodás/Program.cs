using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;

namespace zh2gyakorolgatásocskodás
{
    class Program
    {
        static string[] urls;
        static XDocument[] xdocs;
        static Task[] tasks;
        static List<Dolgozo>[] dlista;
        static List<Dolgozo> chillesdolgozo;
        static void DownloadXML(object data)
        {
            int id = (int)data;
            xdocs[id] = XDocument.Load(urls[id]);
        }
        static void Main(string[] args)
        {
            urls = new string[3];
            urls[0] = "http://users.nik.uni-obuda.hu/kovacs.andras/haladoprog/data/bmf.xml";
            urls[1] = "http://users.nik.uni-obuda.hu/kovacs.andras/haladoprog/data/kmf.xml";
            urls[2] = "http://users.nik.uni-obuda.hu/kovacs.andras/haladoprog/data/kimf.xml";

            xdocs = new XDocument[urls.Length];
            tasks = new Task[urls.Length];

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = new Task(DownloadXML, i);
                tasks[i].Start();
            }

            Task.WhenAll(tasks).ContinueWith( t =>
            { 

                chillesdolgozo = CigisUnio(ConvertMonvert(xdocs));

                MappaGo(chillesdolgozo);
                Display(chillesdolgozo);
            }).Wait();
            Console.ReadLine();
        }


        static List<Dolgozo> [] ConvertMonvert(XDocument[] x)
        {
            List<Dolgozo> [] dlista = new List<Dolgozo>[x.Length];

            for (int i = 0; i < x.Length; i++)
            {
                dlista[i] = new List<Dolgozo>();
                foreach (XElement item in x[i].Descendants("person"))
                {
                    Dolgozo dick = new Dolgozo()
                    {
                        email = item.Element("email").Value,
                        name = item.Element("name").Value
                    };
                    dlista[i].Add(dick);


                }
            }
            return dlista;
        }

        static List<Dolgozo> CigisUnio(List<Dolgozo>[] feladat)
        {
            List<Dolgozo> retur = new List<Dolgozo>();
            for (int i = 0; i < feladat.Length; i++)
            {
                for (int j = 0; j < feladat[i].Count; j++)
                {
                    var item = feladat[i][j];

                    var q = retur.Where(t => t.name == item.name).FirstOrDefault();
                    if (q==null)
                    {
                        string old = item.email.Substring(item.email.IndexOf('@')+1, item.email.LastIndexOf('.') - item.email.IndexOf('@')-1);
                        item.email = item.email.Replace(old, "bmf");
                        retur.Add(item);

                    }
                }
            }
            return retur;
        }
        

        static void MappaGo(List<Dolgozo> lista)
        {
            foreach (var item in lista)
            {
                if (!Directory.Exists(Directory.GetCurrentDirectory() + "/" + item.email));
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/" + item.email);
            }
        }

        static void Display(List<Dolgozo> lista)
        {
            for (int i = 0; i < lista.Count; i++)
            {
                Console.WriteLine("["+i+"]" + lista[i].name);
            }
            Console.Write("\n Send e-mail to (id) : ");
            int id = int.Parse(Console.ReadLine());

            var selected = lista.ElementAt(id);

            string arguments = Directory.GetCurrentDirectory() + "/" + selected.email;
            string directory = Environment.CurrentDirectory;
            string[] temp = directory.Split(new string[] { "\\" }, StringSplitOptions.None);
            int j = 0;
            while (temp[j] != "zh2gyakorolgatásocskodás")
            {
                j++;
            }
            string sumString = "";
            int z = 1;
            sumString += temp[0];
            while (z <= j)
            {
                sumString += "\\" + temp[z];
                z++;
            }
            string temp2 = "\\EmailGo\\bin\\Debug\\EmailGo.exe";
            string temp3 = sumString + temp2;
            temp3.Replace("\\", @"\");
            Process p = new Process()
            {
                StartInfo = new ProcessStartInfo(temp3.Replace("\\", @"\"), arguments)
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
