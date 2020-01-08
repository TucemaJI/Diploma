using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace Finder
{

    public class FinderService
    {
        static bool pause;
        EventWaitHandle waitHandler = new AutoResetEvent(pause);

        public void Search(string locationToSearch)
        {
            waitHandler.WaitOne();

            if (!locationToSearch.EndsWith("\\"))
            {
                locationToSearch += '\\';
            }

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
                    var task = new Task(() => Search(subdirectory));
                    task.Start();
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
