using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailGo
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length > 0)
            {
                string path = args[0];
                Console.Write("Your email: ");
                string email = Console.ReadLine();
                Console.Write("\nYour message: ");
                string bullshit = Console.ReadLine();

                StreamWriter sw = new StreamWriter(path + "/" + email + ".txt", true, Encoding.Default);
                sw.WriteLine(DateTime.Now.ToString() + ": " + bullshit);
                sw.Close();
            }
            else
            {
                Console.WriteLine("Not enough parameters");
            }
        }
    }
}
