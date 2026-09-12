using System;
using System.IO;

namespace Ck
{
    public class Program
    {
        public static int Main(string[] args)
            {
                if (args.Length == 2 && args[0] == "--tokenize")
                {
                    return RunFile(args[1]);
                }
                else if (args.Length == 1)
                {
                    // Lab 0 legacy:  "./run <path>" with no flag
                    Console.WriteLine("Hello, World!");
                    return 0;
                }
                else if (args.Length == 0)
                {
                    RunRepl();
                    return 0;
                }
                else
                {
                    return 64; //command line usage error
                }
            }

        private static int RunFile(string path)
        {
            string source;
            try
            {
                source = File.ReadAllText(path);
            }
            catch (IOException)
            {
                return 66; //cannot open input
            }

            var scanner = new Scanner(source);
            var tokens = scanner.scanTokens();

            if (scanner.hadError)
            {
                return 65;
            }


            foreach (var token in tokens)
            {
                Console.WriteLine(token);
            }
            return 0;
        }

        private static void RunRepl()
        {
            while (true)
            {
                Console.Write("> ");
                string? line = Console.ReadLine();
                if (line is null) break; //eof

                var scanner = new Scanner(line);
                var tokens = scanner.scanTokens();

                foreach (var token in tokens)
                {
                    Console.WriteLine(token);
                }
            }
        }
    }
}
