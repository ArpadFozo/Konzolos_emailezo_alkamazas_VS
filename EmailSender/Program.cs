using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length > 0)
            {
                string sendto = args[0];
                Console.Write("Your e-mail: ");
                string yourEmail = Console.ReadLine();
                Console.Write("Your message: ");
                string yourMessage = Console.ReadLine();
                StreamWriter sw = new StreamWriter(sendto + "/" + yourEmail + ".txt", true,Encoding.Default);
                sw.WriteLine(DateTime.Now.ToString() +" : " + yourMessage);
                sw.Close();
            }
        }
    }
}
