using System.Collections.Generic;
using Xunit;

namespace CSP.Tests
{
    public class VariableTests
    {
        [Fact()]
        public void VariableTest()
        {
            var variable = new Variable(4);
            Assert.Equal(4, variable.AvalibleColors.Count);
            for (int i = 0; i < variable.AvalibleColors.Count; i++)
            {
                for (int j = i + 1; j < variable.AvalibleColors.Count; j++)
                {
                    Assert.NotEqual(variable.AvalibleColors[i].Value, variable.AvalibleColors[j].Value);
                }
            }
        }

        [Fact()]
        public void VariableTest1()
        {
            var colors = new List<Color>() { new Color(2), new Color(4), new Color(1) };
            var variable = new Variable(colors);
            Assert.Equal(colors.Count, variable.AvalibleColors.Count);
            for (int i = 0; i < colors.Count; i++)
            {
                Assert.Contains(colors[i], variable.AvalibleColors);
            }
        }
    }
}