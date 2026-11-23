using System;
using System.Collections.Generic;

namespace Ck
{
    public class NativeError : Exception
    {
        public NativeError(string message) : base(message) { }
    }
    

    public class NoteFunction : ICallable
    {
        private static readonly string[] Names =
            //num of semitones in an octave
            { "C","C#","D","D#","E","F","F#","G","G#","A","A#","B" };
            //flat notes not yet implemented bc it would break the array and the function

        public int ParamsCount() => 1;

        public object? Call(Evaluator evaluator, List<object?> args)
        {
            if (args[0] is not double d)
                throw new NativeError("note() expects a number.");
            int n = (int)Math.Round(d, MidpointRounding.AwayFromZero);
            if (n < 0 || n > 127)
                throw new NativeError("note() expects a value from 0 to 127.");
            return Names[n % 12] + (n / 12 - 1); // 60 -> "C4"
        }
    }

    public class TransposeFunction:ICallable
    {
        public int ParamsCount() => 2; // (note, semitones)

        public object? Call(Evaluator evaluator, List<object?> args)
        {
            if (args[0] is not string note)
                throw new NativeError("transpose() expects a note string.");
            if (args[1] is not double semitones)
                throw new NativeError("transpose() expects a number.");

            throw new NotImplementedException("transpose() is not yet implemented.");

            //TODO: figure out how to implement transpose func
        }
}
}