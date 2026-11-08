using System.Collections.Generic;

namespace Ck
{
    public class Environment
    {
        //the values dictionary is the local scope, and the enclosing is the outer scope
        private readonly Dictionary<string, object?> values = new();
        private readonly Environment? enclosing;

        public Environment(Environment? enclosing = null)
        {
            this.enclosing = enclosing;
        }

        //always local, never walks outward
        public void define(string name, object? value) => values[name] = value;

        public object? get(Token name)
        {
            if (values.TryGetValue(name.Lexeme, out var v)) return v;
            if (enclosing != null) return enclosing.get(name);
            throw new RuntimeError(name, $"Undefined variable '{name.Lexeme}'.");
        }
    }
}