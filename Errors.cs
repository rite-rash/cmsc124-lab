using System;

namespace Ck
{
    public static class Errors
    {
        //TODO: refactor HadError to be a list of errors, so we can report multiple errors at once
        //TODO: refactor HadError to HadStaticError


        public static bool HadError { get; private set; } = false;

        public static void ReportError(int line, string message)
        {
            HadError = true;
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
            HadError = false;
            HadRuntimeError = false;
        }
    }
}