using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP
{
    public struct Pair
    {
        public Pair(Variable variable, Color color)
        {
#if DEBUG
            if (!variable.AvalibleColors.Contains(color))
            {
                throw new ArgumentException("Pair cannot be made, becouse the color does not belong to the variable");
            }
#endif
            Variable = variable;
            Color = color;
        }
        public Variable Variable;
        public Color Color;

        public void Deconstruct(out Variable variable, out Color color)
        {
            variable = Variable;
            color = Color;
        }
    }
}
