using System;
using System.Collections.Generic;
using System.Text;

namespace AXLibrary
{
    public enum Variable
    {
        a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z
    }

    public sealed class ExpressionContext
    {
        private Dictionary<Variable, double> _bindings;

        public ExpressionContext()
        {
            _bindings = new Dictionary<Variable, double>();
        }

        public void Bind(Variable variable, double value)
        {
            _bindings[variable] = value;
        }

        public double Lookup(Variable variable)
        {
            if (_bindings.ContainsKey(variable))
                return _bindings[variable];
            else
                return double.NaN;
        }
    }
}
