using System;

namespace Ck
{
    public static class Errors
    {
        //TODO: refactor HadError to HadStaticError


        public static bool HadStaticError { get; private set; } = false;

        public static void ReportStaticError(int line, string message)
        {
            HadStaticError = true;
            Console.Error.WriteLine($"[line {line}] Error: {message}");
        }

        public static bool HadRuntimeError { get; private set; } = false;

        public static void ReportRuntimeError(int line, string message)
        {
            HadRuntimeError = true;
            Console.Error.WriteLine($"[line {line}] Runtime error: {message}");
        }

        //called before each run, resets error state
        public static void Reset()
        {
            HadStaticError = false;
            HadRuntimeError = false;
        }
    }
}