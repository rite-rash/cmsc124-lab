// DESCRIPTION: traverses the AST and computes actual values

using System;
namespace Ck
{
    public class Evaluator
    {
        // -------- HELPERS --------
        private bool isTruthy(object? v)
        {
            if (v == null) 
                return false;     //rest or null
            if (v is bool b) 
                return b;       //live/mute or true/false
            return true;        //default
        }

        private bool isEqual(object? a, object? b) => Equals(a, b); //compares by value, since == uses references


        //check node type, recursion
        public object? evaluate(Node node)
        {
            switch (node)
            {
                case Literal l: return l.Value;
                case Grouping g: return evaluate(g.Expression);
                case Binary b: return evalBinary(b);
                case Unary u: return evalUnary(u);
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
                case TokenType.GREATER:       checkNumberOperands(op, left, right); return (double)left! >  (double)right!;
                case TokenType.GREATER_EQUAL: checkNumberOperands(op, left, right); return (double)left! >= (double)right!;
                case TokenType.LESS:          checkNumberOperands(op, left, right); return (double)left! <  (double)right!;
                case TokenType.LESS_EQUAL:    checkNumberOperands(op, left, right); return (double)left! <= (double)right!;
                case TokenType.EQUAL_EQUAL:   return isEqual(left, right);
                case TokenType.CHECK_IF:      return !isEqual(left, right);
            }
            throw new RuntimeError(op, "Unknown binary operator.");
        }

        private object? evalUnary(Unary u)
        {
            object? right = evaluate(u.Right);
            switch (u.Operator.Type)
            {
                case TokenType.MINUS:
                    if (right is double d) return -d;
                    throw new RuntimeError(u.Operator, "Operand must be a number.");
                case TokenType.BANG:
                    return !isTruthy(right); //negate
            }
            throw new RuntimeError(u.Operator, "Unknown unary operator.");
        }

        private void checkNumberOperands(Token op, object? left, object? right)
        {
            if (left is double && right is double) return;
            throw new RuntimeError(op, "Operands must be numbers.");
        }

        public string? interpret(Node node)
        {
            try { return stringify(evaluate(node)); }
            catch (RuntimeError e)
            {
                Errors.ReportError(e.Operator.Line, e.Message);
                return null;
            }
        }

        private string stringify(object? v)
        {
            if (v == null) return "rest";
            if (v is bool b) return b ? "live" : "mute";
            if (v is double d) return AstPrinter.PrintNumber(d);
            return v.ToString() ?? "";     // strings print without quotes
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