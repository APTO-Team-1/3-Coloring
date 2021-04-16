using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP
{
    public class Variable
    {
        public Variable(int avalibleColorsCount)
        {
            for (int i = 0; i < avalibleColorsCount; i++)
            {
                this.AvalibleColors.Add(new Color(i));
            }
        }

        public Variable(IEnumerable<Color> avalibleColors)
        {
            this.AvalibleColors.AddRange(avalibleColors);
        }

        public List<Color> AvalibleColors { get; set; } = new List<Color>();
    }
}
