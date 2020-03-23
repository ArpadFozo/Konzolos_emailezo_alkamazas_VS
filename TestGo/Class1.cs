using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zhGyakorlas2;

namespace TestGo
{
    [TestFixture]
    public class Class1
    {
        List<Worker>[] list;

        [SetUp]
        public void inIT()
        {
            list = new List<Worker>[2];

            list[0] = new List<Worker>();
            list[1] = new List<Worker>();

            list[0].Add(new Worker()
            {
                name = "Béres József",
                email = "beres.jozsef@bmf.hu"
            });
            list[0].Add(new Worker()
            {
                name = "Kulcsár János",
                email = "kulcsar.janos@bmf.hu"
            });
            list[1].Add(new Worker()
            {
                name = "Kertész Károly",
                email = "kertesz.karoly@bmf.hu"
            });
            list[1].Add(new Worker()
            {
                name = "Kertész Károly",
                email = "kertesz.karoly@bmf.hu"
            });
        }
        [Test]
        public void DuplicateTest()
        {
            List<Worker> testlista = Program.Union(list);
            Assert.That(testlista.Count == 3);
        }
        [Test]
        public void EmailTest()
        {
            List<Worker> testLista = Program.Union(list);
            int counter = 0;
            foreach (var item in testLista)
            {
                string oldshit = item.email.Substring(item.email.IndexOf('@') + 1, item.email.LastIndexOf('.') - item.email.IndexOf('@') - 1);
                if(oldshit != "bmf")
                {
                    counter++;
                }
            }
            Assert.That(counter == 0);
        }
    }
}
