using System.Linq.Expressions;

namespace Ck
{
    public abstract class Node
    {}

    public class Literal : Node
    {
        public object? Value { get; }

        public Literal(object? value)
        {
           Value = value;
        }
    }

    public class Binary : Node
    {
        public Node Left { get; }
        public Token Operator { get; }
        public Node Right { get; }

        public Binary(Node left, Token op, Node right)
        {
            Left = left;
            Operator = op;
            Right = right;
        }
    }

    public class Grouping : Node
    {
        public Node Expression { get; }

        public Grouping(Node expression)
        {
            Expression = expression;
        }
    }

    public class Unary : Node
    {
        public Token Operator { get; }
        public Node Right { get; }

        public Unary(Token op, Node right)
        {
            Operator = op;
            Right = right;
        }
    }
}