//DESCRIPTION: abstract class for polymorphism
//              blueprint is made for node objects


namespace Ck
{
    public class Variable : Node
    {
        public Token Name { get; }
        public Variable(Token name) { Name = name; }
    }
    
    public abstract class Node
    { }

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


    //get root of the parsed:
    public class Tree : Node
    {
        public List<Statement> Statements  { get; }

        public Tree(List<Statement> statements)
        {
            Statements  = statements ;
        }
    }

    //statements
    public abstract class Statement
    { }
    public class StreamStatement : Statement
    {
        public Node Expression { get; }
        public StreamStatement(Node expression)
        {
            Expression = expression;
        }
    }

    public class ExpressionStatement : Statement
    {
        public Node Expression { get; }
        public ExpressionStatement(Node expression)
        {
            Expression = expression;
        }
    }


    //conditionals
    public class IfStatement : Statement
    {
        //read only properties
        public Node Condition { get; } // root node
        public Statement IfBranch { get; }
        public Statement? ElseBranch { get; } //optional else branch

        public IfStatement(Node condition, Statement ifBranch, Statement? elseBranch){
            Condition = condition;
            IfBranch = ifBranch;
            ElseBranch = elseBranch;

        }



    }
    public class TrackStatement : Statement
{
    public Token Name { get; }
    public Node? Initializer { get; }     //null for `track x;`

    public TrackStatement(Token name, Node? initializer)
    {
        Name = name;
        Initializer = initializer;
    }
}
}




