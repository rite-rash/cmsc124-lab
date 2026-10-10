using System;
using System.IO;
using System.Collections.Generic;

namespace Ck
{
    public class Program
    {

//------ HELPER ------
        private static List<Node>? checkSource(string source)
        {
            var tokens = new Scanner(source).scanTokens();
            if (Errors.HadError){
                return null;
            }
            var nodes = new Parser(tokens).parse();

            if (Errors.HadError){
                return null;
            }

            return nodes;
        }
        //command line
        public static int Main(string[] args)
        {
            if (args.Length == 2 && args[0] == "--tokenize")
                return RunScan(args[1]);
            else if (args.Length == 2 && args[0] == "--parse")
            {
                return RunParse(args[1]);
            }

            if (args.Length == 1)
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

//scanner
        private static int RunScan(string path)
        {
            string source;
            try
            {
                source = File.ReadAllText(path);
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
            {
                return 66; //cannot open input
            }

            var tokens = new Scanner(source).scanTokens();

            if (Errors.HadError)
                return 65; //data format error

            foreach (var token in tokens)
                Console.WriteLine(token);

            return 0;
        }
//parser
        private static int RunParse(string path)
        {
            string source;
            try
            {
                source = File.ReadAllText(path);
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
            {
                return 66; //cannot open input
            }

            var nodes = checkSource(source);
            if (nodes == null) {
                return 65;
            };

            foreach (var node in nodes)
                Console.WriteLine(AstPrinter.Print(node));

            return 0;
        }


        private static void RunRepl()
        {
            while (true)
            {
                Console.Write("> ");
                string? line = Console.ReadLine();
                if (line is null) break; //EOF

                Errors.Reset();
                var nodes = checkSource(line);
                if (nodes == null) continue;

                foreach (var node in nodes)
                {
                    Console.WriteLine(AstPrinter.Print(node));
                }
            }
        }
    }
}