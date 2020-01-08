using System;
using System.Threading.Tasks;

namespace Finder
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = GetPath();
            var service = new FinderService();

            
            var task = new Task(() => service.Search(path));
            task.Start();

            while (true)
            {
                service.Pause();
            }
        }

        private static string GetPath()
        {
            Console.Write("Please enter the directory where is the file (like D:\\ ):");
            string path = Console.ReadLine();
            Console.WriteLine("Press Enter to Continue");
            Console.WriteLine("Press Space to Pause");
            return path;
        }
    }
}
