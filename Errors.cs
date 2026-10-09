using System;

namespace Ck
{
    public static class Errors
    {
        public static bool HadError { get; private set; } = false;

        public static void ReportError(int line, string message)
        {
            HadError = true;
            Console.Error.WriteLine($"[line {line}] Error: {message}");
        }

        //this is called before each line
        public static void Reset()
        {
            HadError = false;
        }
    }
}