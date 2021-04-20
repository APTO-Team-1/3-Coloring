using CSP;
using Xunit;

namespace CSP.Tests
{
    public class RestrictionTests
    {
        [Fact()]
        public void ContainsTest()
        {
            var color1 = new Color(1);
            var color2 = new Color(2);
            var variable1 = new Variable(new[] { color1 });
            var variable2 = new Variable(new[] { color2 });
            var restriction = new Restriction(variable1, color1, variable2, color2);
            Assert.True(restriction.Contains(color1));
            Assert.True(restriction.Contains(color2));
            var color3 = new Color(3);
            var color22 = new Color(2);
            Assert.False(restriction.Contains(color3));
            Assert.False(restriction.Contains(color22));
        }

        [Fact()]
        public void ContainsTest2()
        {
            var color1 = new Color(1);
            var color2 = new Color(2);
            var variable1 = new Variable(new[] { color1 });
            var variable2 = new Variable(new[] { color2 });
            var restriction = new Restriction(variable1, color1, variable2, color2);
            Assert.True(restriction.Contains(variable1));
            Assert.True(restriction.Contains(variable2));
            var variable3 = new Variable(3);
            var variable4 = new Variable(new[] { color2 });
            Assert.False(restriction.Contains(variable3));
            Assert.False(restriction.Contains(variable4));
        }

        [Fact()]
        public void EqualsTest()
        {
            var color1 = new Color(1);
            var color2 = new Color(2);
            var variable1 = new Variable(new[] { color1 });
            var variable2 = new Variable(new[] { color2 });
            var restriction = new Restriction(variable1, color1, variable2, color2);
            var restriction2 = new Restriction(variable1, color1, variable2, color2);
            Assert.True(restriction == restriction2);
            Assert.Equal(restriction, restriction2);
        }

        [Fact()]
        public void NotEqualsTest()
        {
            var color1 = new Color(1);
            var color2 = new Color(2);
            var color22 = new Color(2);
            var color3 = new Color(3);
            var variable1 = new Variable(new[] { color1 });
            var variable2 = new Variable(new[] { color2, color3, color22 });
            var restriction = new Restriction(variable1, color1, variable2, color2);
            var restriction2 = new Restriction(variable1, color1, variable2, color3);
            Assert.False(restriction == restriction2);
            Assert.NotEqual(restriction, restriction2);

            var restriction3 = new Restriction(variable1, color1, variable2, color22);
            Assert.False(restriction == restriction3);
            Assert.NotEqual(restriction, restriction3);
        }

      
    }
}