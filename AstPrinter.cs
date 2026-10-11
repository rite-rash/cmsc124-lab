using System;
using System.Globalization;
using System.Text;

namespace Ck
{
    public static class AstPrinter
    {
        public static string Print(Node node)
        {
            //check type of node
            switch (node)
            {
                case Literal l:
                    return PrintLiteral(l.Value);
                case Grouping g:
                    return Build("group", g.Expression);
                case Unary u:
                    return Build(u.Operator.Lexeme, u.Right);
                case Binary b:
                    return Build(b.Operator.Lexeme, b.Left, b.Right);
                default:
                    throw new InvalidOperationException(
                        "AstPrinter: unknown node " + node?.GetType().Name);
            }
        }

        private static string Build(string name, params Node[] parts)
        {//the tree loop thing
            var sb = new StringBuilder();
            sb.Append('(').Append(name);
            foreach (Node p in parts)
            {
                sb.Append(' ').Append(Print(p));
            }
            sb.Append(')');
            return sb.ToString();
        }

        private static string PrintLiteral(object value)
        {
            if (value == null) return "nil";
            if (value is bool b) return b ? "true" : "false";
            if (value is double d) return PrintNumber(d);
            return value.ToString();
        }

        //numbers always show decimal points (ADD TO README)
        private static string PrintNumber(double d)
        {
            string s = d.ToString(CultureInfo.InvariantCulture);
            bool plain = s.IndexOfAny(new[] { '.', 'E', 'N', 'I' }) < 0;
            return plain ? s + ".0" : s;
        }
    }
}