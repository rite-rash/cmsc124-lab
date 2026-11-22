using System.Collections.Generic;


namespace Ck
{
    public interface ICallable{
        int ParamsCount(); //for evaluator to know num of args
        object? Call(Evaluator evaluator, List<object?> args);
    }
}