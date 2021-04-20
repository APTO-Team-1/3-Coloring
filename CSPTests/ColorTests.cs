using Xunit;
using CSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP.Tests
{
    public class ColorTests
    {
        [Fact()]
        public void ColorTest()
        {
            var color = new Color(1);
            Assert.Equal(1, color.Value);
        }

        [Fact()]
        public void ColorTest2()
        {
            var restrictions = new List<Color>() { new Color(2), new Color(4), new Color(1) };
            var color = new Color(1, restrictions);
            Assert.Equal(color.Restrictions.Count, restrictions.Count);
            foreach (var c in restrictions)
            {
                Assert.Contains(c, color.Restrictions);
            }
        }

      
    }
}