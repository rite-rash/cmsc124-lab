using System;
using System.IO;
using System.Collections.Generic;

namespace Ck
{
    public class Program
    {

//------ HELPER ------
        private static Tree? checkSource(string source)
        {
            var tokens = new Scanner(source).scanTokens();
            if (Errors.HadError)
            {
                return null;
            }

            Tree tree = new Parser(tokens).parse();

            if (Errors.HadError){
                return null;
            }

            return tree;
        }
        //red command line flag
        public static int Main(string[] args)
        {
            if (args.Length == 2 && args[0] == "--tokenize")
                return RunScan(args[1]);
            else if (args.Length == 2 && args[0] == "--parse")
                return RunParse(args[1]);
            else if (args.Length == 2 && args[0] == "--eval")
                return RunEval(args[1]);

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

            var tree = checkSource(source);
            if (tree == null) return 65;

            foreach (var expr in tree.Expressions)
                Console.WriteLine(AstPrinter.Print(expr));
            return 0;
        }

        //evaluator
        private static int RunEval(string path)
        {
            string source;
            try
            {
                source = File.ReadAllText(path);
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
            {
                return 66;
            }

            var tree = checkSource(source);
            if (tree == null) return 65;

            var evaluator = new Evaluator();
            foreach (var expr in tree.Expressions)
            {
                string? text = evaluator.interpret(expr);
                if (text == null) return 70;
                Console.WriteLine(text);
            }
            return 0;

        }

        private static void RunRepl()
        {
            var evaluator = new Evaluator();

            while (true)
            {
                Console.Write("> ");
                string? line = Console.ReadLine();
                if (line is null) 
                    break; //EOF

                Errors.Reset();
                var tree = checkSource(line);
                if (tree == null) 
                    continue;

                foreach (var expr in tree.Expressions)
                {
                    string? text = evaluator.interpret(expr);
                    if (text == null) 
                    break;
                    Console.WriteLine(text);
                }
        }
        }


    }
}