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

        // for native func where no token is required and just registered by name
        public void define(string name, object? value) {
            value[name] = value
        } 


        //always local, never walks outward
        public void define(Token name, object? value)
        {
            if (values.ContainsKey(name.Lexeme))
                throw new RuntimeError(name, $"Already a variable named '{name.Lexeme}' in this scope.");
            values[name.Lexeme] = value;
        }

        public void assign(Token name, object? value)
        {
            if (values.ContainsKey(name.Lexeme)) { values[name.Lexeme] = value; return; }
            if (enclosing != null) { enclosing.assign(name, value); return; }
            throw new RuntimeError(name, $"Undefined variable '{name.Lexeme}'.");
        }

        public object? get(Token name)
        {
            if (values.TryGetValue(name.Lexeme, out var v)) return v;
            if (enclosing != null) return enclosing.get(name);
            throw new RuntimeError(name, $"Undefined variable '{name.Lexeme}'.");
        }


    }
}