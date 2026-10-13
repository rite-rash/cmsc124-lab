// DESCRIPTION: traverses the AST and computes actual values

using System;
namespace Ck
{
    public class Evaluator
    {

        //check node type, recursion
        public object? evaluate(Node node)
        {
            switch (node)
            {
                case Literal l: return l.Value;
                case Grouping g: return evaluate(g.Expression);
                case Binary b: return evalBinary(b);
                default: throw new InvalidOperationException("Evaluator: unknown node " + node?.GetType().Name);
            }
        }

        private object? evalBinary(Binary b)
        {
            //evaluate both sides first
            object? left = evaluate(b.Left);
            object? right = evaluate(b.Right);
            Token op = b.Operator;
            switch (op.Type)
            {
                case TokenType.MIX:
                    if (left is double && right is double) return (double)left! + (double)right!;
                    if (left is string && right is string) return (string)left! + (string)right!;
                    throw new RuntimeError(op, "Operands must be two numbers or two strings.");
                case TokenType.MINUS: checkNumberOperands(op, left, right); return (double)left! - (double)right!;
                case TokenType.STAR: checkNumberOperands(op, left, right); return (double)left! * (double)right!;
                case TokenType.SLASH:
                    checkNumberOperands(op, left, right);
                    if ((double)right! == 0) throw new RuntimeError(op, "Division by zero.");
                    return (double)left! / (double)right!;
            }
            throw new RuntimeError(op, "Unknown binary operator.");
        }

        private void checkNumberOperands(Token op, object? left, object? right)
        {
            if (left is double && right is double) return;
            throw new RuntimeError(op, "Operands must be numbers.");
        }
    }
    public class RuntimeError : Exception
    {
        public Token Operator;
        public RuntimeError(Token op, string message) : base(message)
        {
            Operator = op;
        }
    }
}