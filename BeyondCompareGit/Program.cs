using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeyondCompareGit
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("No directory parameter given.");
                Console.ReadLine();
            }
            String directory = args[0];
            Console.WriteLine($"Start directory: {directory}");
            String directory_git = Path.GetFullPath(Path.Combine(directory, @".git"));
            for (int dir_level = 0; dir_level < 4; dir_level++)
            {
                if (Directory.Exists(directory_git))
                    break;
                directory = Path.GetFullPath(Path.Combine(directory, @"../"));
                directory_git = Path.GetFullPath(Path.Combine(directory, @".git"));
            }
            if (Directory.Exists(directory_git))
            {
                Console.WriteLine("Found git");
                Console.WriteLine(directory_git);
                String config_file = directory_git + "\\config";
                if (File.Exists(config_file))
                {
                    Console.WriteLine("Found git config file.");
                    string contents = File.ReadAllText(config_file);
                    if (contents.Contains("BCompare.exe"))
                    {
                        Console.WriteLine("This git is already configured to use Beyond Compare");
                        Console.ReadLine();
                    }

                    else if (contents.Contains("[diff]"))
                    {
                        Console.WriteLine("This git is already configured to use specific git tool");
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine($"Configure {config_file} to use Beyond Compare Y/N?");

                        if (Console.ReadLine().ToString().ToLower() == "y")
                        {
                            contents = contents +
                                "\n[diff]\n" +
                                "    tool = bc4\n" +
                                "[difftool \"bc4\"]\n" +
                                "    cmd = \\\"C:\\\\Program Files\\\\Beyond Compare 4\\\\BCompare.exe\\\" \\\"$LOCAL\\\" \\\"$REMOTE\\\"\n" +
                                "[merge]\n" +
                                "    tool = bc4\n" +
                                "[mergetool \"bc4\"]\n" +
                                "    cmd = \\\"C:\\\\Program Files\\\\Beyond Compare 4\\\\BCompare.exe\\\" \\\"$REMOTE\\\" \\\"$LOCAL\\\" \\\"$BASE\\\" \\\"$MERGED\\\"\n";
                            File.WriteAllText(config_file, contents);
                            Console.WriteLine($"{config_file} has been updated to use Beyond Compare Y/N?");
                            Console.ReadLine();
                        }
                    }
                }
            }
        }
    }
}
