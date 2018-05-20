using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Finder
{
    class Program
    {

        static void Main(string[] args)
        {
            string path = GetPath();
            var service = new FinderService();

            var thread = new Thread(new ParameterizedThreadStart(service.Search));
            thread.Start(path);

            service.Pause();
        }

        private static string GetPath()
        {
            Console.Write("Please enter the directory where is the file (like D:\\ ):");
            string path = Console.ReadLine();
            return path;
        }
    }
}
