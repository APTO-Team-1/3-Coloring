using System.Collections.Generic;
using Xunit;

namespace CSP.Tests
{
    public class ColorTests
    {
        [Fact()]
        public void ConstructorTest()
        {
            var color = new Color(1);
            Assert.Equal(1, color.Value);
        }

        [Fact()]
        public void ConstructorTest2()
        {
            var variable = new Variable(3);
            var restrictions = new List<Pair>() {
                new Pair(variable, variable.AvalibleColors[0]),
                new Pair(variable, variable.AvalibleColors[1]),
                new Pair(variable, variable.AvalibleColors[2]),
            };
            var color = new Color(1, restrictions);
            Assert.Equal(color.Restrictions.Count, restrictions.Count);
            foreach (var c in restrictions)
            {
                Assert.Contains(c, color.Restrictions);
            }
        }


    }
}