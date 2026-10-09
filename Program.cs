using System;
using System.IO;

namespace Ck
{
    public class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length == 2 && args[0] == "--tokenize")
                return RunFile(args[1]);
            }
            else if (args.Length == 1)
            {
                //lab 0 legacy: "./run <path>" with no flag
                Console.WriteLine("Hello, World!");
                return 0;
            }

            if (args.Length == 0)
            {
                RunRepl();
                return 0;
            }

            return 64;
        }

        private static int RunFile(string path)
        {
            string source;
            try
            {
                source = File.ReadAllText(path);
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
            {
                return 64; //command line usage error
            }
        }

            Scanner.HadError = false;
            var tokens = new Scanner(source).scanTokens();

            if (Scanner.HadError)
                return 65; //data format error

            foreach (var token in tokens)
                Console.WriteLine(token);

            return 0;
        }

        private static void RunRepl()
        {
            while (true)
            {
                Console.Write("> ");
                string? line = Console.ReadLine();
                if (line is null) break; //EOF

                Scanner.HadError = false;
                var tokens = new Scanner(line).scanTokens();

                // print tokens even on error
                foreach (var token in tokens)
                    Console.WriteLine(token);
            }
        }
    }
}