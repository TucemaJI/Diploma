using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace Finder
{

    public class FinderService
    {
        static bool pause;
        EventWaitHandle waitHandler = new AutoResetEvent(pause);

        public void Search(object location)
        {
            waitHandler.WaitOne();
            string locationToSearch = location.ToString();

            if (!locationToSearch.EndsWith("\\"))
            {
                locationToSearch += '\\';
            }

            string result = string.Empty;
            var files = new List<string>();
            var directories = new List<string>();

            string[] fileEntries = Directory.GetFiles(locationToSearch);
            foreach (string file in fileEntries)
            {
                files.Add(file);
            }
            ToConsole(files);

            string[] subdirectoryEntries = Directory.GetDirectories(locationToSearch);

            foreach (string subdirectory in subdirectoryEntries)
            {
                ToConsole(subdirectory);
                var subdirs = Directory.GetAccessControl(subdirectory);
                if (!subdirs.AreAccessRulesProtected)
                {
                    var thread = new Thread(new ParameterizedThreadStart(Search));
                    thread.IsBackground = true;
                    thread.Start(subdirectory);
                }
            }
            waitHandler.Set();
        }

        public void Pause()
        {
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.Spacebar:
                    waitHandler.WaitOne();
                    pause = false;
                    break;
                case ConsoleKey.Enter:
                    waitHandler.Set();
                    pause = true;
                    break;
            }
        }

        public void ToConsole(List<string> files)
        {
            foreach (var file in files)
            {
                Console.WriteLine(file);
            }
        }

        public void ToConsole(string foulder)
        {
            Console.WriteLine(foulder);
        }
    }
}
