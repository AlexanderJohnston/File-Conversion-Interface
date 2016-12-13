using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string folder = "config";
            string configName = @"\users.cfg";
            string[] emptyContentStructure = { "Users:", "", "Passwords:", "", "Flags:", "" };
            bool folderExists = Directory.Exists(path + folder);

            if (folderExists == false)
            {
                Directory.CreateDirectory(path + folder);
                Console.WriteLine(path + folder);
                Console.WriteLine("Created directory.");
            }
            else
            {
                Console.WriteLine("Already exists.");
                Console.WriteLine(path + folder);
            }
            path = Console.ReadLine();
            System.IO.File.WriteAllLines(path + folder + configName, emptyContentStructure);
        }
    }
}
