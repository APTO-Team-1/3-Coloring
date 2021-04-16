using Xunit;
using CSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP.Tests
{
    public class RestrictionTests
    {
        [Fact()]
        public void ContainsTest()
        {
            var color1 = new Color(1);
            var color2 = new Color(2);
            var restriction = new Restriction(color1, color2);
            Assert.True(restriction.Contains(color1));
            Assert.True(restriction.Contains(color2));
            var color3 = new Color(3);
            var color22 = new Color(2);
            Assert.False(restriction.Contains(color3));
            Assert.False(restriction.Contains(color22));
        }

        [Fact()]
        public void EqualsTest()
        {
            var color1 = new Color(1);
            var color2 = new Color(2);
            var restriction = new Restriction(color1, color2);
            var restriction2 = new Restriction(color1, color2);
            Assert.True(restriction == restriction2);
            Assert.Equal(restriction,restriction2);
        }

        [Fact()]
        public void NotEqualsTest()
        {
            var color1 = new Color(1);
            var color2 = new Color(2);
            var color3 = new Color(3);
            var restriction = new Restriction(color1, color2);
            var restriction2 = new Restriction(color1, color3);
            Assert.False(restriction == restriction2);
            Assert.NotEqual(restriction, restriction2);

            var color22 = new Color(2);
            var restriction3 = new Restriction(color1, color22);
            Assert.False(restriction == restriction3);
            Assert.NotEqual(restriction, restriction3);
        }

    }
}